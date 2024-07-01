using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingBackEnd.Entities;
using ParkingBackEnd.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ParkingBackEnd.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParkingController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateParkingWithAddress([FromBody] ParkingWithAddressDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (userEmail == null)
            {
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");
            }

            if (loginType != "parkingAdmin")
            {
                return Unauthorized("Only admins can create new parking!");
            }
            var address = new Address
            {
                City = model.City,
                Street = model.Street,
                PostalCode = model.PostalCode
            };

            _context.Addresses.Add(address);
            _context.SaveChanges();

            ParkingAdmin parkingAdmin = _context.ParkingAdmins.FirstOrDefault(pa => pa.Email == userEmail);
            var parking = new Parking
            {
                Name = model.Name,
                NumberParkingSpaces = model.NumberParkingSpaces,
                NumberFreeParkingSpaces = model.NumberParkingSpaces,
                AddressId = address.Id,
                ParkingAdminId = parkingAdmin.Id
            };

            _context.Parkings.Add(parking);
            _context.SaveChanges();
    
            return Created($"api/Parking", parking);
        }

        [HttpGet]
        public ActionResult<ParkingDto> GetParkings() 
        {

            var parkingsDtos = _context.Parkings.Include(p => p.Address).ToList();
            return Ok(parkingsDtos);
        }


        [Authorize]
        [HttpGet("freeplaces")]
        public IActionResult GetFreeSpaces()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can add drivers to parkings!");

            Parking? parking = _context.Parkings.FirstOrDefault(p => p.ParkingAdmin.Email == adminEmail);

            if (parking == null)
                return NotFound($"Cannot find parking, which belongs to admin with email: {adminEmail}");

            int maxNumParkingPlaces = parking.NumberParkingSpaces;

            List<int> takenPlaces = _context.ParkingAndDrivers.Where(ph => ph.ParkingId == parking.Id).Select(ph => ph.ParkingPlaceNumber).ToList();

            List<int> allPlaces = Enumerable.Range(1, maxNumParkingPlaces).ToList();

            List<int> freePlaces = allPlaces.Except(takenPlaces).ToList();

            return Ok(freePlaces);
        }

        [Authorize]
        [HttpGet("flashingplaces")]
        public IActionResult GetFlashingPlaces()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can add drivers to parkings!");

            Parking? parking = _context.Parkings.FirstOrDefault(p => p.ParkingAdmin.Email == adminEmail);

            if (parking == null)
                return NotFound($"Cannot find parking, which belongs to admin with email: {adminEmail}");


            List<int> flashingPlaces = _context.ParkingAndDrivers.Where(ph => ph.ParkingId == parking.Id && ph.ShouldFlash).Select(ph => ph.ParkingPlaceNumber).ToList();

            return Ok(flashingPlaces);
        }

        [Authorize]
        [HttpGet("takenPlaces")]
        public IActionResult GetTakenSpaces()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can add drivers to parkings!");

            Parking? parking = _context.Parkings.FirstOrDefault(p => p.ParkingAdmin.Email == adminEmail);

            if (parking == null)
                return NotFound($"Cannot find parking, which belongs to admin with email: {adminEmail}");

            int maxNumParkingPlaces = parking.NumberParkingSpaces;

            List<int> takenPlaces = _context.ParkingAndDrivers.Where(ph => ph.ParkingId == parking.Id).Select(ph => ph.ParkingPlaceNumber).ToList();

            return Ok(takenPlaces);
        }



        [Authorize]
        [HttpGet("user/{licensePlateNumber}")]
        public IActionResult GetDriverPlace(string licensePlateNumber)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can add drivers to parkings!");

            Parking? parking = _context.Parkings.FirstOrDefault(p => p.ParkingAdmin.Email == adminEmail);

            if (parking == null)
                return NotFound($"Cannot find parking, which belongs to admin with email: {adminEmail}");

            Driver? driver = _context.Drivers.FirstOrDefault(d => d.LicensePlateNumber == licensePlateNumber);
            if (driver == null)
                return NotFound($"Cannot find driver with license plate number: {licensePlateNumber}");

            int? place = _context.ParkingAndDrivers.Where(pad => pad.ParkingId == parking.Id && pad.DriverId == driver.Id).Select(pad => pad.ParkingPlaceNumber).FirstOrDefault();

            if (place == null)
                return NotFound($"Driver with license plate number: {licensePlateNumber} is not parked in parking with id: {parking.Id}");

            return Ok(place);
        }

        [HttpGet("{id}")]
        public IActionResult GetParkingById(int id)
        {
            var parking = _context.Parkings
                .Include(p => p.Address) 
                .FirstOrDefault(p => p.Id == id);

         
            if (parking == null)
            {
                return NotFound("Can't find parking with provided id");
            }

            return Ok(parking);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteParkingById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can delete parkings!");

            var parking = _context.Parkings.Include(p => p.Address).Include(p => p.ParkingAdmin).FirstOrDefault(p => p.Id == id);

            if (parking == null)
                return NotFound("Can't find parking with provided id"); 
            if (parking.ParkingAdmin.Email != adminEmail)
                return Unauthorized("Only admins, who created parking can delete them!");

            if (parking.Address != null)
                _context.Addresses.Remove(parking.Address);
            _context.Parkings.Remove(parking);
            _context.SaveChanges();

            return NoContent(); 
        }

        [Authorize]
        [HttpPost("driver/{licensePlateNumber}")]
        public IActionResult AddDriverToParking(string licensePlateNumber, [FromBody] TimeAndPlaceDto timeAndPlaceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int maxDurationTimeSeconds = timeAndPlaceDto.MaxSecondsParking;
            int parkingPlace = timeAndPlaceDto.ParkingPlace;

            Driver? driver = _context.Drivers.FirstOrDefault(d => d.LicensePlateNumber == licensePlateNumber);
            if (driver == null)
                return NotFound($"Cannot find driver with license plate number: {licensePlateNumber}");

            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can add drivers to parkings!");

            Parking? parking = _context.Parkings.FirstOrDefault(p => p.ParkingAdmin.Email == adminEmail);

            if (parking == null)
                return NotFound($"Cannot find parking, which belongs to admin with email: {adminEmail}");

            if (parking.NumberParkingSpaces == 0)
                return BadRequest("There are no more free parking space!");

            ParkingAndDrivers? parkingPlaceDb = _context.ParkingAndDrivers.FirstOrDefault(p => p.ParkingId == parking.Id && p.ParkingPlaceNumber == parkingPlace);
            if (parkingPlaceDb != null)
                return BadRequest("This parking place is already taken!");

            if (parkingPlace < 1 || parkingPlace > parking.NumberParkingSpaces)
                return BadRequest("Parking place should be higher, than 1 and less than number of parking spaces!");



            parking.NumberFreeParkingSpaces -= 1;
            _context.ParkingAndDrivers.Add(new ParkingAndDrivers { Driver = driver, Parking = parking, ParkingPlaceNumber = parkingPlace });
            _context.ParkingHistory.Add(new ParkingHistory { 
                Driver = driver, Parking = parking, EntryDate = DateTime.Now, MaxTimeInSeconds = maxDurationTimeSeconds, ParkingPlaceNumber = parkingPlace });
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpDelete("driver/{licensePlateNumber}")]
        public IActionResult DeleteDriverFromParking(string licensePlateNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Driver? driver = _context.Drivers.FirstOrDefault(d => d.LicensePlateNumber == licensePlateNumber);
            if (driver == null)
            {
                return NotFound($"Cannot find driver with license plate number: {licensePlateNumber}");
            }

            var adminEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var loginType = User.Claims.FirstOrDefault(c => c.Type == "LoginType")?.Value;

            if (adminEmail == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            if (loginType != "parkingAdmin")
                return Unauthorized("Only admins can add drivers to parkings!");

            Parking parking = _context.Parkings.FirstOrDefault(p => p.ParkingAdmin.Email == adminEmail);

            if (parking == null)
            {
                return NotFound($"Cannot find parking, which belongs to admin with email: {adminEmail}");
            }

            ParkingAndDrivers? parkingAndDrivers = _context.ParkingAndDrivers.FirstOrDefault(pad => pad.ParkingId == parking.Id && pad.DriverId == driver.Id);

            if (parkingAndDrivers == null)
            {
                return BadRequest($"There are not driver with licence plate number {licensePlateNumber} on parking with id {parking.Id}");
            }
            ParkingHistory parkingHistoryRecord = _context.ParkingHistory.FirstOrDefault(ph => ph.Parking == parking && ph.Driver == driver && ph.ExitDate == null);
            
            if (parkingHistoryRecord == null)
            {
                return BadRequest($"User with licence plate number: {licensePlateNumber} is not in the parking");
            }
            DateTime now = DateTime.Now;
            parkingHistoryRecord.ExitDate = now;

            TimeSpan timeDifference = now - parkingHistoryRecord.EntryDate;
            
            parking.NumberFreeParkingSpaces += 1;
            _context.ParkingAndDrivers.Remove(parkingAndDrivers);
            _context.SaveChanges();
            int secondsLeft = parkingHistoryRecord.MaxTimeInSeconds - (int)timeDifference.TotalSeconds;
            return Ok(secondsLeft);
        }



        //[Authorize]
        //[HttpGet("takenPlaces")]
        //public IActionResult GetFreeSpaces()
        //{

        //}

        //[Authorize]
        //[HttpGet("userPlace")]
        //public IActionResult GetFreeSpaces()
        //{

        //}
    }
}
