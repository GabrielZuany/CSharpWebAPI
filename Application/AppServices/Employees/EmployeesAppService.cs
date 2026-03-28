using Application.AppServices.Employees.Interfaces;
using Application.DTOs.Requests.Employees;
using Application.Services.S3Services.Interfaces;
using Domain.Employees.Model;
using Domain.Employees.Services;
using Microsoft.Extensions.Logging;

namespace Application.AppServices.Employees;

public class EmployeesAppService : IEmployeesAppService
{
    private readonly IS3Service _s3Service;
    private readonly IEmployeeService employeeService;
    private readonly ILogger<EmployeesAppService> logger;
    public EmployeesAppService(IEmployeeService employeeService, IS3Service s3Service, ILogger<EmployeesAppService> logger)
    {
        this.employeeService = employeeService;
        this._s3Service = s3Service;
        this.logger = logger;
    }

    public async Task CreateAsync(AddEmployeeRequest request)
    {
        string bucketName = "test-zuzu-webapi-s3dotnet";
        string key = request.Photo.FileName;

        try
        {
            using Stream fileStream = request.Photo.OpenReadStream();
            bool result = await _s3Service.PutObjectAsync(bucketName, key, fileStream);
            string s3publicURL = _s3Service.GetObjectPublicURL(bucketName, key);
            employeeService.Add(request.Fullname, request.Age, request.Postalcode, s3publicURL);
        }
        catch (Exception ex)
        {   
            logger.LogError(ex, "Error occurred while uploading file and saving employee details");
            throw ex;
        }
    }
}