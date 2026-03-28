using Domain.Employees.Model;

namespace Domain.Employees.Repository
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        Task<Employee?>? Get(Guid id);
        Task<List<Employee>> GetAll(int page, int size);
    }
}
