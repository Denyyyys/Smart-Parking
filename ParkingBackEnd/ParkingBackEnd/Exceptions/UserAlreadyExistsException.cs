namespace ParkingBackEnd.Exceptions
{
    public class UserAlreadyExistsException: GeneralAPIException
    {
        public UserAlreadyExistsException(string message = "User with provided email already exists!") : base(message)
        {
            StatusCode = 409;
        }
    }
}
