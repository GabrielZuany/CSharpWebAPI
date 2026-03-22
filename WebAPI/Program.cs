using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
using Domain.Users.Repository;
using Domain.Users.Service;
using Infrastructure.Employees.Repository;
using Infrastructure.Employees.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Users.Repository;
using Infrastructure.Users.Services;
using ConnectionContext = Infrastructure.Persistence.ConnectionContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("AWSSettings"));

// Add services to the container.
string _connectionString = "Host=localhost;Username=webapi;Password=webapi;Database=postgres";  
builder.Services.AddDbContext<ConnectionContext>(options =>
        options.UseNpgsql(_connectionString)
            .EnableSensitiveDataLogging() // optional
);

builder.Services.AddControllers();
// injects the repository into the controller automatically
// so we don't need to create a new instance of the repository in the controller constructor every time

builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();

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


// Configure the HTTP request pipeline.
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

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
