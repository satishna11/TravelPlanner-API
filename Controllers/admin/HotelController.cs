using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class HotelController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public HotelController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _context.Hotels.Include(h => h.Destination).ToListAsync();
            return Success(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Destination)
                .FirstOrDefaultAsync(h => h.HotelId == id);

            if (hotel == null)
                return NotFoundResponse("Hotel not found");

            return Success(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return Success(hotel, "Hotel created");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Hotel hotel)
        {
            var existing = await _context.Hotels.FindAsync(id);

            if (existing == null)
                return NotFoundResponse("Hotel not found");

            existing.HotelName = hotel.HotelName;
            existing.PricePerNight = hotel.PricePerNight;
            existing.Rating = hotel.Rating;
            existing.Category = hotel.Category;
            existing.Facilities = hotel.Facilities;
            existing.ImageUrl = hotel.ImageUrl;
            existing.DestinationId = hotel.DestinationId;

            await _context.SaveChangesAsync();
            return Success(existing, "Hotel updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
                return NotFoundResponse("Hotel not found");

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return Success(null, "Hotel deleted");
        }
    }
}