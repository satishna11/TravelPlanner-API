using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class TripDestinationController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public TripDestinationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.TripDestinations
                .Include(td => td.Trip)
                .Include(td => td.Destination)
                .ToListAsync();

            return Success(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TripDestination td)
        {
            _context.TripDestinations.Add(td);
            await _context.SaveChangesAsync();

            return Success(td, "Trip destination added");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var td = await _context.TripDestinations.FindAsync(id);

            if (td == null)
                return NotFoundResponse("Not found");

            _context.TripDestinations.Remove(td);
            await _context.SaveChangesAsync();

            return Success(null, "Deleted");
        }
    }
}
