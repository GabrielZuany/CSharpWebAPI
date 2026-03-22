using Application.AppServices.Employees.Interfaces;
using Application.DTOs.Requests.Employees;
using Domain.Employees.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesAppService employeesAppService;
        private readonly IEmployeeService employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeesAppService employeesAppService, IEmployeeService employeeService,ILogger<EmployeeController> logger)
        {
            this.employeesAppService = employeesAppService ?? throw new ArgumentNullException(nameof(employeesAppService));
            this.employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]AddEmployeeRequest request)
        {
            try
            {
                await employeesAppService.CreateAsync(request);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error adding employee");
                return StatusCode(500);
            }
        }
        
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageQuantity)
        {
            try
            {
                var employees = await employeeService.GetAll(pageNumber, pageQuantity);
                return Ok(employees);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error getting employees");
                return StatusCode(500);
            }            
        }

        // [Authorize]
        // [HttpPost("{id}/download-photo")]
        // public IActionResult DownloadPhoto(Guid id)
        // {
        //     try
        //     {
        //         var employee = _employeeRepository.Get(id);
        //         var dataBytes = System.IO.File.ReadAllBytes(employee.Photo);
        //         return File(dataBytes, "image/jpeg");
        //     }
        //     catch(Exception e)
        //     {
        //         _logger.LogError(e, "Error downloading photo");
        //         return StatusCode(500);
        //     }
        // }

        
    }
}
