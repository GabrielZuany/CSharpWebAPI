using Domain.Users.Model;

namespace Domain.Users.Repository;

public interface IUsersRepository
{
    Task<User?> FindByUsernameAndPassword(string username, string password);
    Task<User?> FindByUsername(string username);
    Task UpdateUser(User user);
}