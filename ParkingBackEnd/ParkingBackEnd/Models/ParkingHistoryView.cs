namespace ParkingBackEnd.Models
{
    public class ParkingHistoryView
    {
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public string ParkingName { get; set; }

        public int ParkingPlace { get; set; }
        public int durationSeconds { get; set; }
    }
}
