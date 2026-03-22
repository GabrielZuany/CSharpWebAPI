using Application.DTOs.Requests.Employees;

namespace Application.AppServices.Employees.Interfaces;

public interface IEmployeesAppService
{
    Task CreateAsync(AddEmployeeRequest request);
}