using Domain.Employees.Model;

namespace Domain.Employees.Services;

public interface IEmployeeService
{
    Task<Employee?> FindByUsernameAndPassword(string username, string password);
}