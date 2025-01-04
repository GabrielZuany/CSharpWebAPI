using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.ViewModel;
using WebAPI.Domain.Repository;
using WebAPI.Domain.Model;
using WebAPI.Domain.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IS3Service _s3Service;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger, IS3Service s3service)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _s3Service = s3service ?? throw new ArgumentNullException(nameof(_s3Service));
        }

        //[Authorize]
        [HttpPost("add-employee")]
        public async Task<IActionResult> Add([FromForm]EmployeeViewModel employeeView)
        {
            string bucketName = "test-zuzu-webapi-s3dotnet";
            string key = employeeView.Photo.FileName;
            try
            {
                using Stream fileStream = employeeView.Photo.OpenReadStream();
                bool result = await _s3Service.PutObjectAsync(bucketName, key, fileStream);
                string s3publicURL = _s3Service.GetObjectPublicURL(bucketName, key);

                var employee = new Employee(employeeView.Fullname, employeeView.Age, employeeView.Postalcode, s3publicURL);
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
