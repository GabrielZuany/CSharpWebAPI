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
            .Where(w => w.Username == username && w.Password == password)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
    
    public async Task<User?> FindById(Guid id)
    {
        return await this._context
            .Users
            .Where(w => w.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
    
    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

}