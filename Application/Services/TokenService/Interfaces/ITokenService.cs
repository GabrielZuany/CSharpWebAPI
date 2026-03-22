namespace Application.Services.TokenService.Interfaces;

public interface ITokenService
{
    object GenerateToken<T>(T objectBase, string secret);
}