using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers
{
    [Route("api/[controller]")]
    public class DestinationController : AdminBaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DestinationController(
            AppDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var destinations = await _context.Destinations
                .Select(d => new DestinationDto
                {
                    DestinationId = d.DestinationId,
                    Name = d.Name,
                    City = d.City,
                    Country = d.Country,
                    Description = d.Description,
                    AverageBudget = d.AverageBudget,
                    ImageUrl = d.ImageUrl,
                    ViewCount = d.ViewCount
                })
                .ToListAsync();

            return Success(destinations);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var destination = await _context.Destinations
                .Where(d => d.DestinationId == id)
                .Select(d => new DestinationDto
                {
                    DestinationId = d.DestinationId,
                    Name = d.Name,
                    City = d.City,
                    Country = d.Country,
                    Description = d.Description,
                    AverageBudget = d.AverageBudget,
                    ImageUrl = d.ImageUrl,
                    ViewCount = d.ViewCount
                })
                .FirstOrDefaultAsync();

            if (destination == null)
                return NotFoundResponse("Destination not found");

            return Success(destination);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DestinationDto dto)
        {
            string imagePath = "";

            if (dto.Image != null)
            {
                var folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "destination");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName =
                    Guid.NewGuid() +
                    Path.GetExtension(dto.Image.FileName);

                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imagePath = "/images/destination/" + fileName;
            }

            var destination = new Destination
            {
                Name = dto.Name,
                City = dto.City,
                Country = dto.Country,
                Description = dto.Description,
                AverageBudget = dto.AverageBudget,
                ImageUrl = imagePath,
                ViewCount = 0
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            return Success(destination, "Destination created successfully.");
        }        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] DestinationDto dto)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
                return NotFoundResponse("Destination not found");

            destination.Name = dto.Name;
            destination.City = dto.City;
            destination.Country = dto.Country;
            destination.Description = dto.Description;
            destination.AverageBudget = dto.AverageBudget;

            if (dto.Image != null)
            {
                var folder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "destination");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(destination.ImageUrl))
                {
                    var oldImagePath = Path.Combine(
                        _environment.WebRootPath,
                        destination.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = Guid.NewGuid() +
                               Path.GetExtension(dto.Image.FileName);

                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                destination.ImageUrl = "/images/destination/" + fileName;
            }

            await _context.SaveChangesAsync();

            return Success(destination, "Destination updated successfully.");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
                return NotFoundResponse("Destination not found");

            // Delete image from wwwroot
            if (!string.IsNullOrEmpty(destination.ImageUrl))
            {
                var imagePath = Path.Combine(
                    _environment.WebRootPath,
                    destination.ImageUrl.TrimStart('/'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return Success(null, "Destination deleted successfully.");
        }
    }
}