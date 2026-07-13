using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;

namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class randomDestinationController : BaseController
    {
        private readonly AppDbContext _context;

        public randomDestinationController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> getRandomDestinations()
        {
            var destinations = await _context.Destinations
                .OrderBy(d => Guid.NewGuid()) // Random order
                .Take(6) // Show 4 destinations
                .Select(d => new
                {
                    d.DestinationId,
                    d.Name,
                    d.City,
                    d.Country,
                    d.Description,
                    d.AverageBudget,
                    d.ImageUrl,
                    d.ViewCount
                })
                .ToListAsync();

            return Success(destinations, "Featured destinations");
        }
    }
}