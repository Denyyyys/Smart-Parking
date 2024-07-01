using System.ComponentModel.DataAnnotations;

namespace ParkingBackEnd.Models
{
    public class LoginDto
    {

        public string? LoginType { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
