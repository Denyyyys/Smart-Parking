using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ParkingBackEnd.Entities
{
    public class Parking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberParkingSpaces { get; set; }
        public int NumberFreeParkingSpaces { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Required]
        public int ParkingAdminId {  get; set; }

        public virtual ParkingAdmin ParkingAdmin { get; set; }

        public ICollection<ParkingHistory> ParkingHistory { get; set; }



    }
}
