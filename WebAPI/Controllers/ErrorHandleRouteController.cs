using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorHandleRouteController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem();

        [Route("/error-local-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if(! hostEnvironment.IsDevelopment())
            {
                return Problem(title: "An error occurred.", detail: "Something went wrong. Please try again later.");
            }
            
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }
    }
}
