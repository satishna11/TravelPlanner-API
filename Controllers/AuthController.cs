using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.Entities;
using TravelAI.Models.DTOs;
using TravelAI.Services;


namespace TravelAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;
        public AuthController(AppDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var exists = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (exists != null)
                return Fail("User already exists");

            var user = new Models.Entities.User
            {
                FullName = request.FullName,
                Email = request.Email,
                UserType = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Success(user, "User registered");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                return Fail("Invalid credentials");

            var validPassword = BCrypt.Net.BCrypt.Verify(
                request.Password, user.PasswordHash);

            if (!validPassword)
                return Fail("Invalid credentials");

            var token = _jwt.GenerateToken(user);

            return Success(new
            {
                token,
                user = new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.UserType
                }
            }, "Login successful");
        }
    }
}