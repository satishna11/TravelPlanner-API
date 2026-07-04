using System.ComponentModel.DataAnnotations;

namespace TravelAI.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? ProfileImage { get; set; }
        
        public string UserType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // public ICollection<Trip>? Trips { get; set; }
    }
}