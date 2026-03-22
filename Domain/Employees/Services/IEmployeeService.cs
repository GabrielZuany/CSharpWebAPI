using Domain.Employees.Model;

namespace Domain.Employees.Services;

public interface IEmployeeService
{
    void Create(string fullname, int age, string postalCode, string s3PublicUrl);
    Task<List<Employee>> GetAll(int page, int size);
}