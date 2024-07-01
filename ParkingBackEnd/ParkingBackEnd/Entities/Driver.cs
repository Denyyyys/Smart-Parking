using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingBackEnd.Entities
{
    //[Table("Users")]
    public class Driver : User
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        public string LicensePlateNumber { get; set; }

        public ICollection<ParkingHistory> ParkingHistory { get; set; }
        //public int RoleId { get; set; } = 1;
        //public virtual Role Role { get; set; }
    }
}
