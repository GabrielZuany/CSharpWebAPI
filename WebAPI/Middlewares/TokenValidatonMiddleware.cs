using System.Security.Claims;
using Application.DTOs.Requests.Auth;
using Application.Services.AuthService.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Middlewares;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var publicRoutes = new[] { "/api/v1/auth" , "/scalar"};
        if (publicRoutes.Any(r => context.Request.Path.StartsWithSegments(r, StringComparison.OrdinalIgnoreCase)
                                  && (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Get)
                                  && !context.Request.Path.Value!.Contains("refresh")))
        {
            await _next(context);
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing token");
            return;
        }

        var userId = authService.ValidateToken(token);
        if (userId != null)
        {
            var claims = new[] { new Claim("UserId", userId.ToString()!) };
            var identity = new ClaimsIdentity(claims, "Bearer");
            context.User = new ClaimsPrincipal(identity);
    
            context.Items["UserId"] = userId;
            context.Response.StatusCode = StatusCodes.Status200OK;
            await _next(context);
            return;
        }

        var refreshToken = context.Request.Headers["Refresh-Token"].FirstOrDefault();
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var currentToken = await authService.FindByToken(refreshToken);
            if (currentToken != null)
            {
                var newRefreshToken = await authService.RefreshToken(new RefreshTokenRequest()
                {
                    UserId = currentToken.UserId,
                    RefreshToken = currentToken.Token
                });

                if (newRefreshToken != null)
                {
                    var claims = new[] { new Claim("claimObject", userId.ToString()!) };
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    context.User = new ClaimsPrincipal(identity);
                    
                    context.Response.Headers.Append("Authorization", $"Bearer {newRefreshToken.AccessToken}");
                    context.Response.Headers.Append("Refresh-Token", newRefreshToken.RefreshToken);
                    context.Items["UserId"] = authService.ValidateToken(newRefreshToken.AccessToken);
                    
                    await _next(context);
                    return;
                }
            }
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Invalid or expired token");
    }
}