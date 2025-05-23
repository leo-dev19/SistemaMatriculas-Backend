using System.ComponentModel.DataAnnotations;

namespace servicio.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string FirtName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
