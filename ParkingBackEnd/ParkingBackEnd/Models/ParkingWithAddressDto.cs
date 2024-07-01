namespace ParkingBackEnd.Models
{
    public class ParkingWithAddressDto
    {
        public string Name { get; set; }
        public int NumberParkingSpaces { get; set; }
        public int NumberFreeParkingSpaces { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

    }
}
