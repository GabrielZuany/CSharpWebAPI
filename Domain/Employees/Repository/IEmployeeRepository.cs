using Domain.Employees.Model;

namespace Domain.Employees.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee?> FindByUsernameAndPassword(string username, string password);
        
        void Add(Employee employee);
        List<Employee> Get(int pageNumber, int pageQuantity);
        Employee? Get(Guid id);
    }
}
