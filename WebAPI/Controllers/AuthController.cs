using Application.DTOs.Requests.Auth;
using Application.Services.AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] AuthLoginRequest request)
        {
            var response = await this.authService.AdminAuthenticated(request);
            return response == null ? Unauthorized() : Ok(response);
        }
    }
}
