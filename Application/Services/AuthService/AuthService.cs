using Application.DTOs.Requests.Auth;
using Application.Services.AuthService.Interfaces;
using Application.Services.TokenService.Interfaces;
using Domain.Employees.Services;
using Domain.Enums;
using Domain.Users.Model;
using Domain.Users.Repository;
using Domain.Users.Service;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ITokenService tokenService;
    private readonly IUsersRepository usersRepository; 
    
    public AuthService(ITokenService tokenService, IUsersRepository usersRepository)
    {
        this.tokenService = tokenService;
        this.usersRepository = usersRepository;
    }

    public async Task<AccessTokenResponse?>? AdminAuthenticated(AuthLoginRequest request)
    {
        var user = await this.usersRepository.FindByUsernameAndPassword(request.Username, request.Password);
        
        if (user == null || user.LevelAccess != LevelAccessEnum.Admin)
            return null;

        var accessToken = this.tokenService.GenerateToken<User>(user, Key.Secret).ToString();
        var expiresAt = DateTime.Now.AddMinutes(5).Ticks;
        var refreshToken = this.tokenService.GenerateRefreshToken(user);
        
        user.RefreshToken = refreshToken;
        await this.usersRepository.UpdateUser(user);

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
        var user = await usersRepository.FindByUsername(request.Username);

        if (user == null || user.RefreshToken != request.RefreshToken)
            return null;

        var newAccessToken = this.tokenService.GenerateToken<User>(user, Key.Secret).ToString();
        var newExpiresAt = DateTime.Now.AddMinutes(5).Ticks;
        var newRefreshToken = this.tokenService.GenerateRefreshToken(user);

        user.RefreshToken = newRefreshToken;
        await usersRepository.UpdateUser(user);

        var response = new AccessTokenResponse()
        {
            AccessToken = newAccessToken,
            ExpiresIn = newExpiresAt,
            RefreshToken = newRefreshToken
        };
        return response;
    }
}