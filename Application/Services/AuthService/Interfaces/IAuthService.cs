using Application.DTOs.Requests.Auth;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Application.Services.AuthService.Interfaces;

public interface IAuthService
{
    Task<AccessTokenResponse?>? AdminAuthenticated(AuthLoginRequest request);
}