namespace ParkingBackEnd.Models
{
    public class TokenClaims
    {
        public string LoginType { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }

    }
}
