namespace TravelAI.Models.DTOs
{
    public class LoginRequest
    {
        public string? FullName{get; set;}=string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}