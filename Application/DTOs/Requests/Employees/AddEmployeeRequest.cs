using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Requests.Employees;

public class AddEmployeeRequest
{
    public string Fullname { get; set; }
    public int Age { get; set; }
    public string Postalcode { get; set; }
    public IFormFile Photo { get; set; }
}