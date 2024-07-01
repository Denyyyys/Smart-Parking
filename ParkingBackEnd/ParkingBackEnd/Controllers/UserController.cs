using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingBackEnd.Entities;
using ParkingBackEnd.Models;
using ParkingBackEnd.Services;

namespace ParkingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly AppDbContext _context;

        public UserController(IIdentityService identityService, AppDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }


        [Authorize]
        [HttpGet("stats")]
        public ActionResult GetStats()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var userIdAsObj = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = int.Parse(userIdAsObj);
            if (userEmail == null || userId == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            Driver driver = _context.Drivers.FirstOrDefault(d => d.Email == userEmail);

            List<ParkingHistory> driverHistoryParkings = _context.ParkingHistory.Where(ph => ph.DriverId == userId && ph.ExitDate != null).ToList();
            


            return Ok(new DriverStats 
                { Email = userEmail, ParkingNumbers = driverHistoryParkings.Count, 
                    FirstName=driver.FirstName, LastName=driver.LastName, LicensePlateNumber=driver.LicensePlateNumber
            });
        }

        [Authorize]
        [HttpGet("history")]
        public ActionResult GetHistory()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var userIdAsObj = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = int.Parse(userIdAsObj);
            if (userEmail == null || userId == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            List<ParkingHistory> driverHistoryParkings = _context.ParkingHistory.Where(ph => ph.DriverId == userId && ph.ExitDate != null).ToList();

            List<ParkingHistoryView> historyUser = new List<ParkingHistoryView>();

            foreach (var historyParking in driverHistoryParkings)
            {
                TimeSpan timeDifference = historyParking.ExitDate.GetValueOrDefault(DateTime.Now) - historyParking.EntryDate;
                Parking p = _context.Parkings.FirstOrDefault(p => p.Id == historyParking.ParkingId);
                int timeInSeconds = timeDifference.Seconds;
                
                historyUser.Add(new ParkingHistoryView
                {
                    EntryDate = historyParking.EntryDate,
                    ExitDate = historyParking.ExitDate,
                    durationSeconds = timeInSeconds,
                    ParkingName = p.Name,
                    ParkingPlace = historyParking.ParkingPlaceNumber
                });
            }

            return Ok(historyUser);
        }

        [Authorize]
        [HttpGet("timeLeft")]
        public ActionResult GeTimeLeft()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var userIdAsObj = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = int.Parse(userIdAsObj);
            if (userEmail == null || userId == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            ParkingHistory? parkingHistory = _context.ParkingHistory.FirstOrDefault(ph => ph.DriverId == userId && ph.ExitDate == null);


            if (parkingHistory == null)
            {
                return Ok("You are not parked");
            }
            TimeSpan timeDifference = parkingHistory.EntryDate.AddSeconds(parkingHistory.MaxTimeInSeconds) - DateTime.Now;

            return Ok((int)timeDifference.TotalSeconds);

        }

        [Authorize]
        [HttpGet("startFlash")]
        public IActionResult FlashSpace(int number)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var userIdAsObj = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = int.Parse(userIdAsObj);
            if (userEmail == null || userId == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            ParkingAndDrivers? parkingToBeFlashed = _context.ParkingAndDrivers.FirstOrDefault(ph => ph.DriverId == userId);

            if (parkingToBeFlashed == null)
                return NotFound($"You are not parked");


            parkingToBeFlashed.ShouldFlash = true;
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpGet("stopFlash")]
        public IActionResult StopFlashSpace(int number)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var userIdAsObj = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = int.Parse(userIdAsObj);
            if (userEmail == null || userId == null)
                return Unauthorized("Your session is expired or you are unauthorized to perform this action!");

            ParkingAndDrivers? parkingToBeFlashed = _context.ParkingAndDrivers.FirstOrDefault(ph => ph.DriverId == userId);

            if (parkingToBeFlashed == null)
                return NotFound($"You are not parked");


            parkingToBeFlashed.ShouldFlash = false;
            _context.SaveChanges();

            return Ok();
        }

    }
}
