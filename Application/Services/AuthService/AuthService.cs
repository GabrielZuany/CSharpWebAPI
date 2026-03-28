using Application.DTOs.Requests.Auth;
using Application.Services.AuthService.Interfaces;
using Application.Services.TokenService.Interfaces;
using Domain.Employees.Services;
using Domain.Enums;
using Domain.RefreshTokens.Model;
using Domain.RefreshTokens.Repository;
using Domain.Users.Model;
using Domain.Users.Repository;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ITokenService tokenService;
    private readonly IUsersRepository usersRepository; 
    private readonly IRefreshTokensRepository refreshTokensRepository; 
    
    public AuthService(ITokenService tokenService, IUsersRepository usersRepository, IRefreshTokensRepository refreshTokensRepository)
    {
        this.tokenService = tokenService;
        this.usersRepository = usersRepository;
        this.refreshTokensRepository = refreshTokensRepository; 
    }

    public async Task<AccessTokenResponse?>? AdminAuthenticated(AuthLoginRequest request)
    {
        var user = await this.usersRepository.FindByUsernameAndPassword(request.Username, request.Password);
        
        if (user == null || user.LevelAccess != LevelAccessEnum.Admin)
            return null;

        var accessToken = this.tokenService.GenerateToken<Guid>(user.Id, Key.Secret).ToString();
        var expiresAt = DateTime.UtcNow.AddMinutes(5).Ticks;
        var refreshToken = this.tokenService.GenerateRefreshToken(user);
        
        var refreshTokenDb = new RefreshToken()
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
        await this.refreshTokensRepository.Add(refreshTokenDb);

        var response = new AccessTokenResponse()
        {
            AccessToken = accessToken,
            ExpiresIn = expiresAt,
            RefreshToken = refreshToken
        };
        return response;
    }

    public async Task<AccessTokenResponse?>? RefreshToken(RefreshTokenRequest request)
    {
        var user = await usersRepository.FindById(request.UserId);
        var refreshToken = await refreshTokensRepository.FindByToken(request.RefreshToken);

        if (user == null ||
            refreshToken == null ||
            !refreshToken.UserId.Equals(request.UserId) ||
            !refreshToken.Token.Equals(request.RefreshToken))
        {
            return null;
        }

        var newAccessToken = this.tokenService.GenerateToken<Guid>(user.Id, Key.Secret).ToString();
        var newExpiresAt = DateTime.UtcNow.AddMinutes(3).Ticks;
        var newRefreshToken = this.tokenService.GenerateRefreshToken(user.Id);
        
        await this.refreshTokensRepository.Add(new RefreshToken()
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = refreshToken.ExpiresAt,
        });
        
        await refreshTokensRepository.Delete(refreshToken);

        var response = new AccessTokenResponse()
        {
            AccessToken = newAccessToken,
            ExpiresIn = newExpiresAt,
            RefreshToken = newRefreshToken
        };
        return response;
    }

    public async Task<RefreshToken?>? FindByToken(string token)
    {
        return await refreshTokensRepository.FindByToken(token);
    }

    public Guid? ValidateToken(string token)
    {
        try
        {
            Guid? userId = tokenService.ValidateToken<Guid>(token);
            if (userId != null && !userId.Equals(Guid.Empty))
            {
                return userId;
            }
        }
        catch (Exception)
        {
            // Handle exception, e.g., log it or return null
        }

        return null;
    }
}