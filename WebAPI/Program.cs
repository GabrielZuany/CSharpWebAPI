
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.AppServices.Employees;
using Application.AppServices.Employees.Interfaces;
using Application.Services.AuthService;
using Application.Services.AuthService.Interfaces;
using Application.Services.S3Services;
using Application.Services.S3Services.Interfaces;
using Application.Services.S3Services.Model;
using Application.Services.TokenService;
using Application.Services.TokenService.Interfaces;
using Domain.Employees.Repository;
using Domain.Employees.Services;
using Domain.RefreshTokens.Repository;
using Domain.Users.Repository;
using Domain.Users.Service;
using Infrastructure.Employees.Repository;
using Infrastructure.Employees.Services;
using Scalar.AspNetCore;
using Infrastructure.RefreshTokens.Repository;
using Infrastructure.Users.Repository;
using Infrastructure.Users.Services;
using WebAPI.Middlewares;
using ConnectionContext = Infrastructure.Persistence.ConnectionContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("AWSSettings"));

// Add services to the container.
string _connectionString = "Host=localhost;Username=webapi;Password=webapi;Database=postgres";  
builder.Services.AddDbContext<ConnectionContext>(options =>
        options.UseNpgsql(_connectionString)
            .EnableSensitiveDataLogging()
);

builder.Services.AddControllers();

// Register RequestDelegate
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IRefreshTokensRepository, RefreshTokensRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IS3Service, S3Service>();
builder.Services.AddTransient<IEmployeesAppService, EmployeesAppService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes(Key.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ConnectionContext>();
    db.Database.Migrate(); 
}

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-local-development");
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });
    
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar"));
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseMiddleware<TokenValidationMiddleware>();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();