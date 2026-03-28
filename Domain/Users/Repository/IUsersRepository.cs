using Domain.Users.Model;

namespace Domain.Users.Repository;

public interface IUsersRepository
{
    Task<User?> FindByUsernameAndPassword(string username, string password);
    Task<User?> FindById(Guid id);
    Task UpdateUser(User user);
    void AddUser(User user);
}