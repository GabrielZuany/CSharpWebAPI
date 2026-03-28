using Application.DTOs.Requests.Auth;
using Domain.RefreshTokens.Model;
using Domain.Users.Model;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Application.Services.AuthService.Interfaces;

public interface IAuthService
{
    Task<AccessTokenResponse?>? AdminAuthenticated(AuthLoginRequest request);
    Task<AccessTokenResponse?>? RefreshToken(RefreshTokenRequest request);

    Guid? ValidateToken(string token);

    Task<RefreshToken?>? FindByToken(string token);

}