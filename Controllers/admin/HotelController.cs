using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class HotelController : AdminBaseController
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public HotelController(
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _context.Hotels
                .Include(h => h.Destination)
                .Select(h => new
                {
                    HotelId = h.HotelId,
                    HotelName = h.HotelName,
                    PricePerNight = h.PricePerNight,
                    Rating = h.Rating,
                    Category = h.Category,
                    Facilities = h.Facilities,
                    ImageUrl = h.ImageUrl,
                    DestinationId = h.DestinationId,
                    DestinationName = h.Destination.Name
                })
                .ToListAsync();

            return Success(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Destination)
                .Where(h => h.HotelId == id)
                .Select(h => new
                {
                    HotelId = h.HotelId,
                    HotelName = h.HotelName,
                    PricePerNight = h.PricePerNight,
                    Rating = h.Rating,
                    Category = h.Category,
                    Facilities = h.Facilities,
                    ImageUrl = h.ImageUrl,
                    DestinationId = h.DestinationId,
                    DestinationName = h.Destination.Name
                })
                .FirstOrDefaultAsync();

            if (hotel == null)
                return NotFoundResponse("Hotel not found");

            return Success(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] HotelDto dto)
        {
            string imagePath = "";

            if (dto.Image != null)
            {
                var folder = Path.Combine(_environment.WebRootPath, "images", "hotels");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);

                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);

                await dto.Image.CopyToAsync(stream);

                imagePath = "/images/hotels/" + fileName;
            }

            var hotel = new Hotel
            {
                HotelName = dto.HotelName,
                PricePerNight = dto.PricePerNight,
                Rating = dto.Rating,
                Category = dto.Category,
                Facilities = dto.Facilities,
                DestinationId = dto.DestinationId,
                ImageUrl = imagePath
            };

            _context.Hotels.Add(hotel);

            await _context.SaveChangesAsync();

            return Success(hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] HotelDto dto)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
                return NotFoundResponse("Hotel not found");

            hotel.HotelName = dto.HotelName;
            hotel.PricePerNight = dto.PricePerNight;
            hotel.Rating = dto.Rating;
            hotel.Category = dto.Category;
            hotel.Facilities = dto.Facilities;
            hotel.DestinationId = dto.DestinationId;

            if (dto.Image != null)
            {
                var folder = Path.Combine(_environment.WebRootPath, "images", "hotels");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                if (!string.IsNullOrEmpty(hotel.ImageUrl))
                {
                    var oldImage = Path.Combine(
                        _environment.WebRootPath,
                        hotel.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImage))
                        System.IO.File.Delete(oldImage);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                hotel.ImageUrl = "/images/hotels/" + fileName;
            }

            await _context.SaveChangesAsync();

            return Success(hotel, "Hotel updated successfully.");
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