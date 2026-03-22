using Domain.Employees.Model;

namespace Domain.Employees.Repository
{
    public interface IEmployeeRepository
    {
        void Create(Employee employee);
        Task<List<Employee>> GetAll(int page, int size);
    }
}
