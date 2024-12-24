using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.ViewModel;
using WebAPI.Domain.Repository;
using WebAPI.Domain.Model;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //[Authorize]
        [HttpPost("add-employee")]
        public IActionResult Add([FromForm]EmployeeViewModel employeeView)
        {
            try
            {
                var filePath = Path.Combine("Storage", employeeView.Photo.FileName);
                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                employeeView.Photo.CopyTo(fileStream);

                var employee = new Employee(employeeView.Fullname, employeeView.Age, employeeView.Postalcode, filePath);
                _employeeRepository.Add(employee);
                return Ok();
            }catch(Exception e)
            {
                _logger.LogError(e, "Error adding employee");
                return StatusCode(500);
            }
        }

        //[Authorize]
        [HttpPost("{id}/download-photo")]
        public IActionResult DownloadPhoto(Guid id)
        {
            try
            {
                var employee = _employeeRepository.Get(id);
                var dataBytes = System.IO.File.ReadAllBytes(employee.Photo);
                return File(dataBytes, "image/jpeg");
            }catch(Exception e)
            {
                _logger.LogError(e, "Error downloading photo");
                return StatusCode(500);
            }
        }

        //[Authorize]
        [HttpGet("get-employees")]
        public IActionResult Get(int pageNumber, int pageQuantity)
        {
            try
            {
                var employees = _employeeRepository.Get(pageNumber, pageQuantity);
                return Ok(employees);
            }catch(Exception e)
            {
                _logger.LogError(e, "Error getting employees");
                return StatusCode(500);
            }            
        }
    }
}
