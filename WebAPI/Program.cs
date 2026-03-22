using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Services;
using WebAPI.Domain.Repository;
using WebAPI.Domain.Services;
using WebAPI.Infra;
using WebAPI.Infra.Repository;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

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

builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IS3Service, S3Service>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes(WebAPI.Key.Secret);
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
