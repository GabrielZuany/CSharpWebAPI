using Domain.Users.Model;
using Domain.Users.Repository;
using Domain.Users.Service;

namespace Infrastructure.Users.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }
    
    public async Task<User?> FindByUsernameAndPassword(string username, string password)
    {
        if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
        if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
        
        return await this.usersRepository.FindByUsernameAndPassword(username, password);
    }
}