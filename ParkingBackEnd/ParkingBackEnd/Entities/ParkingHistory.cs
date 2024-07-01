using System.ComponentModel.DataAnnotations;

namespace ParkingBackEnd.Entities
{
    public class ParkingHistory
    {
        public int Id { get; set; } // Primary Key

        [Required]
        public int ParkingId { get; set; }
       
        [Required]
        public int DriverId { get; set; }

        [Required]
        public DateTime EntryDate { get; set; } // Timestamp for entry
        public DateTime? ExitDate { get; set; } // Timestamp for exit (nullable)

        [Required]
        public int MaxTimeInSeconds { get; set; } // Duration stored in seconds

        public int ParkingPlaceNumber { get; set; }
        public virtual Parking Parking { get; set; }
        public virtual Driver Driver { get; set; }
    }
}