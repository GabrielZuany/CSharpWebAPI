namespace Application.Services.TokenService.Interfaces;

public interface ITokenService
{
    object GenerateToken<T>(T objectBase, string secret);
    string GenerateRefreshToken<T>(T entity);
    T ValidateToken<T>(string token);
}