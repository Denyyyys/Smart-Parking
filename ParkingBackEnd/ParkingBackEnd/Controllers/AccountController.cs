using Microsoft.AspNetCore.Mvc;
using ParkingBackEnd.Models;
using ParkingBackEnd.Services;
using ParkingBackEnd.Models;
using ParkingBackEnd.Services;
using System.Text.Json;
using ParkingBackEnd.Entities;
using ParkingBackEnd.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ParkingBackEnd.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityService _identityService;
        private readonly AppDbContext _context;


        public AccountController(IAccountService accountService, IIdentityService identityService, AppDbContext context)
        {
            _accountService = accountService;
            _identityService = identityService;
            _context = context;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] dynamic  model)
        {
            RegisterUserDto registerUserDto = JsonSerializer.Deserialize<RegisterUserDto>(model);
            if (registerUserDto.RegistrationType == "driver")
            {
                RegisterDriverDto driverModel = JsonSerializer.Deserialize<RegisterDriverDto>(model);
                Driver newDriver = _accountService.RegisterDriver(driverModel);
                var token = _identityService.GenerateToken(new TokenClaims { Email = newDriver.Email, UserId = newDriver.Id, LoginType = "driver" });
                return Created("api/register", new { token = token });
            }
            else if (registerUserDto.RegistrationType == "parkingAdmin")
            {
                RegisterParkingAdminDto parkingAdminModel = JsonSerializer.Deserialize<RegisterParkingAdminDto>(model);
                ParkingAdmin newParkingAdmin =  _accountService.RegisterParkingAdmin(parkingAdminModel);
                var token = _identityService.GenerateToken(new TokenClaims { Email = newParkingAdmin.Email, UserId = newParkingAdmin.Id, LoginType = "parkingAdmin" });
                return Created("api/register", new { token = token });
            }
            else
            {
                return BadRequest($"Provided registrationType is not supported! Provided: {registerUserDto.RegistrationType}");
            }
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            if (loginDto.LoginType == "driver")
            {
                var driver = _accountService.LoginDriver(loginDto.Email, loginDto.Password);
                if (driver != null)
                {
                    return Ok(driver);
                }
            }
            else if (loginDto.LoginType == "parkingAdmin")
            {
                var admin = _accountService.LoginParkingAdmin(loginDto.Email, loginDto.Password);
                if (admin != null)
                {
                    return Ok(admin);
                }
            }
            else
            {

                var loginResult = _accountService.Login(loginDto.Email, loginDto.Password);
                dynamic result = loginResult;
                var user = result.user;
                var loginType = result.LoginType;

                //User? user = _context.ParkingAdmins.FirstOrDefault(pa => pa.Email == loginDto.Email);
                //string LoginType = "parkingAdmin";
                //if (user == null)
                //{
                //    user = _context.Drivers.FirstOrDefault(pa => pa.Email == loginDto.Email);
                //    LoginType = "driver";
                //}
                if (user != null) 
                {
                    var token = _identityService.GenerateToken(new TokenClaims { Email = user.Email, UserId = user.Id, LoginType = loginType});
                    if (token != "")
                    {
                        return Ok(new { token = token });
                    }
                }


            }
            throw new BadCredentialsException("Wrong Email Or Password!");
        }
    }
}