using WebAPI.Domain.Model;
using WebAPI.DTOs;

namespace WebAPI.Domain.Repository
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        List<EmployeeDTO> Get(int pageNumber, int pageQuantity);
        Employee? Get(Guid id);
    }
}
