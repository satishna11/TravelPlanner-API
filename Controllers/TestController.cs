using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TravelAI.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Public");
    }

    [Authorize]
    [HttpGet("auth")]
    public IActionResult Auth()
    {
        return Ok(User.Claims.Select(x => new
        {
            x.Type,
            x.Value
        }));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        return Ok("Admin");
    }
}