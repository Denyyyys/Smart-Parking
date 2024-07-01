namespace ParkingBackEnd.Exceptions
{
    public class GeneralAPIException: Exception
    {
        public int StatusCode { get; set; }
        public GeneralAPIException(string message) : base(message) { }

    }
}
