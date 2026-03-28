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

    public void Add(string fullname, int age, string postalCode, string s3PublicUrl)
    {
        if (string.IsNullOrEmpty(s3PublicUrl))
            throw new ArgumentNullException(nameof(s3PublicUrl));
        
        var employee = new Employee(fullname, age, postalCode, s3PublicUrl);
        employeeRepository.Add(employee);
    }

    public async Task<List<Employee>> GetAll(int page, int size)
    {
        if (page < 0) page = 0;
        return await this.employeeRepository.GetAll(page, size);
    }
}