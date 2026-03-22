using Application.DTOs.Requests.Auth;
using Application.Services.AuthService.Interfaces;
using Application.Services.TokenService.Interfaces;
using Domain.Employees.Model;
using Domain.Employees.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ITokenService tokenService;
    private readonly IEmployeeService employeeService;

    public AuthService(ITokenService tokenService, IEmployeeService employeeService)
    {
        this.tokenService = tokenService;
    }

    public async Task<AccessTokenResponse?>? AdminAuthenticated(AuthLoginRequest request)
    {
        var employee = await this.employeeService.FindByUsernameAndPassword(request.Username, request.Password);
        
        if (employee == null || employee.LevelAccess != LevelAccessEnum.Admin)
            return null;
        
        var token = this.tokenService.GenerateToken<Employee>(employee, Key.Secret).ToString();
        var response = new AccessTokenResponse()
        {
            AccessToken = token,
            ExpiresIn = DateTime.Now.AddHours(3).Ticks,
            RefreshToken =  token
        };
        return response;
    }
}