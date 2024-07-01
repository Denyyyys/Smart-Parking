using ParkingBackEnd.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingBackEnd.Models
{
    public class RegisterUserDto
    {
        [Required]
        public string RegistrationType {  get; set; }
        //"registrationType": "driver"
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}