using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAI.Data;
using TravelAI.Models.DTOs;
using TravelAI.Models.Entities;

namespace TravelAI.Controllers.admin
{
    [Route("api/[controller]")]
    public class UserController:AdminBaseController
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL USERS
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Success(users);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFoundResponse("User not found");

            return Success(user);
        }

        // CREATE USER
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] LoginRequest request)
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Success(user, "User Created Successfully");
        }

        // UPDATE USER
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Models.Entities.User user)
        {
            var existing = await _context.Users.FindAsync(id);

            if (existing == null)
                return NotFoundResponse("User not found");

            existing.FullName = user.FullName;
            existing.Email = user.Email;
            
            existing.UserType = user.UserType;

            await _context.SaveChangesAsync();

            return Success(existing, "User updated");
        }

        // DELETE USER
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFoundResponse("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Success(null, "User deleted");
        }
    }
}