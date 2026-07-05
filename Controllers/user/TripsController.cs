using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelAI.Data;

namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/user/[controller]")]
    [Authorize(Roles = "User")]
    public class TripController : BaseController
    {
        private readonly AppDbContext _context;

        public TripController(AppDbContext context)
        {
            _context = context;
        }

        // Get all trips of logged in user
        [HttpGet("my-trips")]
        public async Task<IActionResult> MyTrips()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var trips = await _context.Trips
                .Where(x => x.UserId == userId)
                .Include(x => x.TripDestinations)
                .ThenInclude(td => td.Destination)
                .OrderByDescending(x => x.TravelDate)
                .ToListAsync();

            return Success(trips);
        }
    }
}