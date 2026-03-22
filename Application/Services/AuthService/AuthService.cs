using Application.DTOs.Requests.Auth;
using Application.Services.AuthService.Interfaces;
using Application.Services.TokenService.Interfaces;
using Domain.Employees.Services;
using Domain.Enums;
using Domain.Users.Model;
using Domain.Users.Service;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ITokenService tokenService;
    private readonly IUsersService usersService;

    public AuthService(ITokenService tokenService, IEmployeeService employeeService)
    {
        this.tokenService = tokenService;
    }

    public async Task<AccessTokenResponse?>? AdminAuthenticated(AuthLoginRequest request)
    {
        var user = await this.usersService.FindByUsernameAndPassword(request.Username, request.Password);
        
        if (user == null || user.LevelAccess != LevelAccessEnum.Admin)
            return null;
        
        var token = this.tokenService.GenerateToken<User>(user, Key.Secret).ToString();
        var response = new AccessTokenResponse()
        {
            AccessToken = token,
            ExpiresIn = DateTime.Now.AddHours(3).Ticks,
            RefreshToken =  token
        };
        return response;
    }
}