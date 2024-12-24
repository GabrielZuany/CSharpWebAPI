using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Services;
using WebAPI.Domain.Model;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        
        [HttpGet]
        public IActionResult Auth(string username, string password)
        {
            // check if the username and password are correct
            // if correct, generate a token
            // if not, return a 401 Unauthorized status code
            if (username == "admin" && password == "admin")
            {
                var token = TokenService.GenerateToken(new Employee());
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
