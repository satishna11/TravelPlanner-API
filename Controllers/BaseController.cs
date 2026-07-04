using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelAI.Controllers
{
  
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Success(object? data = null, string message = "Success")
        {
            return Ok(new
            {
                success = true,
                message,
                data
            });
        }

        protected IActionResult Fail(string message)
        {
            return BadRequest(new
            {
                success = false,
                message,
                data = (object?)null
            });
        }

        protected IActionResult NotFoundResponse(string message = "Not found")
        {
            return NotFound(new
            {
                success = false,
                message,
                data = (object?)null
            });
        }
    }
}