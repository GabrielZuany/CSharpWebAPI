using Domain.Users.Model;
using Domain.Users.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users.Repository;

public class UsersRepository : IUsersRepository
{
    private readonly ConnectionContext _context;

    public UsersRepository(ConnectionContext context)
    {
        _context = context;
    }
    
    public async Task<User?> FindByUsernameAndPassword(string username, string password)
    {
        return await this._context
            .Users
            .Where(w => w.Username == username && w.Password.ToString() == password)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

}