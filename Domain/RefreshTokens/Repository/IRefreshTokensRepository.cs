using Domain.RefreshTokens.Model;

namespace Domain.RefreshTokens.Repository;

public interface IRefreshTokensRepository
{
    Task Add(RefreshToken token);
    Task<RefreshToken?>? FindByToken(string token);
}