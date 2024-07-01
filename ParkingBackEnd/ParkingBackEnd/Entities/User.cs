using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingBackEnd.Entities
{
    //[Table("Users")]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        //public int RoleId { get; set; } = 1;
        //public virtual Role Role { get; set; }
    }
}
