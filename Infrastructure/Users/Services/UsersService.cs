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
        return await this.usersRepository.FindByUsernameAndPassword(username, password);
    }
}