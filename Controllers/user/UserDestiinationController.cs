using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;

namespace TravelAI.Controllers.User
{
    [ApiController]
    [Route("api/user/destination")]
    [Authorize(Roles = "User")]
    public class UserDestinationController : BaseController
    {
        private readonly AppDbContext _context;

        public UserDestinationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDestinations()
        {
            var destinations = await _context.Destinations
                .OrderBy(d => d.Name)
                .ToListAsync();

            return Success(destinations);
        }
    }
}