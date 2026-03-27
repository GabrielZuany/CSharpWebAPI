using Domain.RefreshTokens.Model;
using Domain.RefreshTokens.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RefreshTokens.Repository;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly ConnectionContext _context;

    public RefreshTokensRepository(ConnectionContext context)
    {
        _context = context;
    }

    public async Task Add(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?>? FindByToken(string token)
    {
        return await _context
            .RefreshTokens
            .Where(w => w.Token == token)
            .FirstOrDefaultAsync();
    }
}