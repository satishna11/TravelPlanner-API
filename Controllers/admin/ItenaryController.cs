using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class ItineraryController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public ItineraryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Itineraries
                .Include(i => i.Trip)
                .ToListAsync();

            return Success(data);
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetByTrip(int tripId)
        {
            var data = await _context.Itineraries
                .Where(i => i.TripId == tripId)
                .ToListAsync();

            if (data == null || data.Count == 0)
                return NotFoundResponse("No itinerary found");

            return Success(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Itinerary itinerary)
        {
            _context.Itineraries.Add(itinerary);
            await _context.SaveChangesAsync();

            return Success(itinerary, "Itinerary created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Itineraries.FindAsync(id);

            if (item == null)
                return NotFoundResponse("Not found");

            _context.Itineraries.Remove(item);
            await _context.SaveChangesAsync();

            return Success(null, "Deleted");
        }
    }
}