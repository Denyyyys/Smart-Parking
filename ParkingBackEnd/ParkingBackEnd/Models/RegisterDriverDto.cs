using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingBackEnd.Models
{

    public class RegisterDriverDto: RegisterUserDto
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        public string LicensePlateNumber { get; set; }

    }
}