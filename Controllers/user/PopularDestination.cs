using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;

namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PopularDestinationController : BaseController
    {
        private readonly AppDbContext _context;
        public PopularDestinationController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetPopularDestinations()
        {
            var popularDestinations = await _context.Destinations
                .Select(d => new
                {
                    d.DestinationId,
                    d.Name,
                    d.City,
                    d.Country,
                    d.Description,
                    d.AverageBudget,
                    d.ImageUrl,
                    d.ViewCount,

                    TotalBookings = _context.TripDestinations
                        .Count(td => td.DestinationId == d.DestinationId)
                })
                .OrderByDescending(x => x.TotalBookings)
                .ThenByDescending(x => x.ViewCount)
                .Take(6)
                .ToListAsync();

            return Success(popularDestinations, "Popular destinations");
        }
    }
}