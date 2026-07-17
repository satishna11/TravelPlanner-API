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
        [HttpGet("{id}/activities")]
        public async Task<IActionResult> GetActivities(int id)
        {
            var destination = await _context.Destinations
                .FirstOrDefaultAsync(x => x.DestinationId == id);

            if (destination == null)
                return Fail("Destination not found");

            var activities = await _context.DestinationActivity
                .Where(x => x.DestinationId == id)
                .Select(a => new
                {
                    a.ActivityName,
                    a.Category,
                    a.TimeSlot,
                    a.DurationHours,
                    a.EstimatedCost,
                    a.ImageUrl
                })
                .ToListAsync();

            return Success(new
            {
                destination.DestinationId,
                destination.Name,
                destination.Description,
                destination.ImageUrl,
                Activities = activities
            });
        }
    }
}