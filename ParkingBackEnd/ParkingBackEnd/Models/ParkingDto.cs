namespace ParkingBackEnd.Models
{
    public class ParkingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberParkingSpaces { get; set; }
        public int NumberFreeParkingSpaces { get; set; }
    }
}
