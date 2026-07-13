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
            var popularDestinations = await _context.TripDestinations
                .GroupBy(td => td.DestinationId)
                .Select(g => new
                {
                    DestinationId = g.Key,
                    TotalBookings = g.Count()
                })
                .Join(
                    _context.Destinations,
                    booking => booking.DestinationId,
                    destination => destination.DestinationId,
                    (booking, destination) => new
                    {
                        destination.DestinationId,
                        destination.Name,
                        destination.City,
                        destination.Country,
                        destination.Description,
                        destination.AverageBudget,
                        destination.ImageUrl,
                        destination.ViewCount,
                        TotalBookings = booking.TotalBookings
                    })
                .OrderByDescending(x => x.TotalBookings)
                .Take(6)
                .ToListAsync();

            return Success(popularDestinations, "Popular destinations");
        }
    }
}