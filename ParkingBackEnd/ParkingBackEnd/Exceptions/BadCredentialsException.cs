namespace ParkingBackEnd.Exceptions
{
    public class BadCredentialsException : GeneralAPIException
    {
        public BadCredentialsException(string message="Bad Credentials Provided!") : base(message) 
        {
            StatusCode = 401;
        }
    }
}
