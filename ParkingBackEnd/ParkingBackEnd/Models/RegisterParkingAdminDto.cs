using ParkingBackEnd.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingBackEnd.Models
{
    public class RegisterParkingAdminDto : RegisterUserDto
    {
        public int ParkingId { get; set; }
        public virtual Parking Parking { get; set; }
    }
}