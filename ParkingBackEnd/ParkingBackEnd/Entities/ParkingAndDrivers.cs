using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingBackEnd.Entities
{
    public class ParkingAndDrivers
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        public int DriverId { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        public int ParkingId { get; set; }

        public int ParkingPlaceNumber { get; set; }
        public bool ShouldFlash { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Parking Parking { get; set; }
    }

}
