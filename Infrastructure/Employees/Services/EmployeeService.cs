using Domain.Employees.Model;
using Domain.Employees.Repository;
using Domain.Employees.Services;

namespace Infrastructure.Employees.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public async Task<Employee?> FindByUsernameAndPassword(string username, string password)
    {
        return await this.employeeRepository.FindByUsernameAndPassword(username, password);
    }

}