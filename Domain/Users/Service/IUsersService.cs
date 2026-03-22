using Domain.Users.Model;

namespace Domain.Users.Service;

public interface IUsersService
{
    Task<User?> FindByUsernameAndPassword(string username, string password);
}