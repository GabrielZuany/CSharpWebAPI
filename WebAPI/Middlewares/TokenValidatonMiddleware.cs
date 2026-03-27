using Application.Services.AuthService.Interfaces;

namespace WebAPI.Middlewares;

public class TokenValidatonMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthService _authService;

    public TokenValidatonMiddleware(RequestDelegate next, IAuthService authService)
    {
        _next = next ?? throw  new ArgumentNullException(nameof(next));
        _authService = authService ?? throw  new ArgumentNullException(nameof(authService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing token");
            return;
        }

        // try
        // {
        //     var user = await _authService.ValidateToken(token);
        //     if (user != null)
        //     {
        //         context.Items["User"] = user;
        //         await _next(context);
        //         return;
        //     }
        // }
        // catch (Exception)
        // {
        //     var refreshToken = context.Request.Headers["Refresh-Token"].FirstOrDefault();
        //     if (!string.IsNullOrEmpty(refreshToken))
        //     {
        //         var newTokens = await _authService.RefreshToken(refreshToken);
        //         if (newTokens != null)
        //         {
        //             context.Response.Headers.Add("Authorization", $"Bearer {newTokens.AccessToken}");
        //             context.Response.Headers.Add("Refresh-Token", newTokens.RefreshToken);
        //
        //             context.Items["User"] = await _authService.ValidateToken(newTokens.AccessToken);
        //             await _next(context);
        //             return;
        //         }
        //     }
        // }

        // context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        // await context.Response.WriteAsync("Invalid token");
    } 

}