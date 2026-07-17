using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelAI.Data;
using TravelAI.Models.DTOs;

namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class HotelRecommendationController : BaseController
    {
        private readonly AppDbContext _context;

        public HotelRecommendationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> RecommendHotels()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var latestTrip = await _context.Trips
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TripId)
                .FirstOrDefaultAsync();

            // ==========================
            // NO TRIP -> Popular Hotels
            // ==========================
            if (latestTrip == null)
            {
                var hotels = await _context.Hotels
                    .OrderByDescending(h => h.Rating)
                    .Take(10)
                    .Select(h => new HotelDto
                    {
                        HotelName = h.HotelName,
                        PricePerNight = h.PricePerNight,
                        Rating = h.Rating,
                        Category = h.Category,
                        Facilities = h.Facilities,
                        DestinationId = h.DestinationId,
                        ImageUrl = h.ImageUrl,
                    })
                    .ToListAsync();

                return Success(hotels, "Popular hotels");
            }

            var tripDestination = await _context.TripDestinations
                .FirstOrDefaultAsync(td => td.TripId == latestTrip.TripId);

            if (tripDestination == null)
            {
                var hotels = await _context.Hotels
                    .OrderByDescending(h => h.Rating)
                    .Take(10)
                    .Select(h => new HotelDto
                    {
                        HotelName = h.HotelName,
                        PricePerNight = h.PricePerNight,
                        Rating = h.Rating,
                        Category = h.Category,
                        Facilities = h.Facilities,
                        DestinationId = h.DestinationId,
                        ImageUrl = h.ImageUrl
                    })
                    .ToListAsync();

                return Success(hotels, "Popular hotels");
            }

            // ==========================
            // Recommended Hotels
            // ==========================
            var recommendedHotels = await _context.Hotels
                .Where(h =>
                    h.DestinationId == tripDestination.DestinationId &&
                    h.Category == latestTrip.HotelCategory)
                .OrderByDescending(h => h.Rating)
                .Select(h => new HotelDto
                {
                    HotelName = h.HotelName,
                    PricePerNight = h.PricePerNight,
                    Rating = h.Rating,
                    Category = h.Category,
                    Facilities = h.Facilities,
                    DestinationId = h.DestinationId,
                    ImageUrl = h.ImageUrl
                })
                .ToListAsync();

            if (!recommendedHotels.Any())
            {
                recommendedHotels = await _context.Hotels
                    .Where(h => h.DestinationId == tripDestination.DestinationId)
                    .OrderByDescending(h => h.Rating)
                    .Select(h => new HotelDto
                    {
                        HotelName = h.HotelName,
                        PricePerNight = h.PricePerNight,
                        Rating = h.Rating,
                        Category = h.Category,
                        Facilities = h.Facilities,
                        DestinationId = h.DestinationId,
                        ImageUrl = h.ImageUrl
                    })
                    .ToListAsync();
            }

            return Success(recommendedHotels, "Recommended hotels");
        }
    }
}