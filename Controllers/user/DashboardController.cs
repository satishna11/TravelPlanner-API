using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelAI.Data;

namespace TravelAI.Controllers.User
{
    [ApiController]
    [Route("api/user/dashboard")]
    [Authorize(Roles = "User")]
    public class DashboardController : BaseController
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Total Trips
            int totalTrips = await _context.Trips
                .CountAsync(t => t.UserId == userId);

            // Total Destinations (including repeated visits)
            int totalDestinations = await _context.TripDestinations
                .CountAsync(td => td.Trip.UserId == userId);

            // Unique Destinations Visited
            int uniqueDestinations = await _context.TripDestinations
                .Where(td => td.Trip.UserId == userId)
                .Select(td => td.DestinationId)
                .Distinct()
                .CountAsync();

            // Upcoming Trips
            int upcomingTrips = await _context.Trips
                .CountAsync(t =>
                    t.UserId == userId &&
                    t.TravelDate >= DateTime.Today);

            return Success(new
            {
                totalTrips,
                totalDestinations,
                uniqueDestinations,
                upcomingTrips
            });
        }
    }
}