using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParkingBackEnd.Entities;
using ParkingBackEnd.Exceptions;
using ParkingBackEnd.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingBackEnd.Services
{
    public interface IAccountService
    {
        public Driver RegisterDriver(RegisterDriverDto dto);
        public ParkingAdmin RegisterParkingAdmin(RegisterParkingAdminDto dto);

        public Driver? LoginDriver(string email, string password);
        public ParkingAdmin? LoginParkingAdmin(string email, string password);

        public object Login(string email, string password);

        //public TokenClaims? GetClaimsFromToken(string token);

    }

    public class AccountService : IAccountService
    {
        //private const string TokenSecret = "secret";
        //private static readonly TimeSpan TokenLifetime = TimeSpan.FromDays(100);


        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        //public TokenClaims? GetClaimsFromToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(TokenSecret);

        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false, // Since we're not including issuer in the token
        //            ValidateAudience = false, // You can optionally set it to true if you want to validate the audience
        //            ClockSkew = TimeSpan.Zero // You can adjust this value according to your requirements
        //        }, out SecurityToken validatedToken);
        //        /*
        //                        return (validatedToken as TokenClaims);*/

        //        var jwtToken = validatedToken as JwtSecurityToken;
        //        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //            throw new SecurityTokenException("Invalid token");

        //        var claims = new Dictionary<string, string>();
        //        foreach (var claim in jwtToken.Claims)
        //        {
        //            claims.Add(claim.Type, claim.Value);
        //        }
        //        var claimss = new TokenClaims
        //        {
        //            LoginType = claims.ContainsKey("LoginType") ? claims["LoginType"] : null,
        //            Email = claims.ContainsKey("email") ? claims["email"] : null,
        //            UserId = claims.ContainsKey("UserId") ? int.Parse(claims["UserId"]) : 0
        //        };
        //        return claimss;
        //    }
        //    catch (SecurityTokenException)
        //    {
        //        throw new Exception("SecurityTokenException");
        //    }
        //}
        //public string GenerateToken(TokenClaims tokenClaims)
        //{
        //    /* var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.UTF8.GetBytes(TokenSecret);
        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(new Claim[]
        //         {
        //             new(JwtRegisteredClaimNames.Email, tokenClaims.Email),
        //             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //             new("UserId", tokenClaims.UserId.ToString()),
        //             new("LoginType", tokenClaims.LoginType),
        //         }),
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //     };
        //     var token = tokenHandler.CreateToken(tokenDescriptor);
        //     return tokenHandler.WriteToken(token);*/
        //    return null;
        //}

        public AccountService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _context = dbContext;
            _passwordHasher = passwordHasher;
        }

        public object Login(string email, string password)
        {
            User? user = LoginDriver(email, password);
            string type = "driver";
            if (user == null){
                user = LoginParkingAdmin(email, password);
                type = "parkingAdmin";
            }
            //if (user == null)
            //    return "";
            //string token = GenerateToken(new TokenClaims { Email = user.Email, LoginType = type, UserId = user.Id });
            return new { user = user, LoginType= type };
        }

        public Driver? LoginDriver(string email, string password)
        {

            var driver = _context.Drivers.FirstOrDefault(d => d.Email == email);

            if (driver != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(driver, driver.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                    return driver;
            }

            return null;
        }

        public ParkingAdmin LoginParkingAdmin(string email, string password)
        {
            var admin = _context.ParkingAdmins.FirstOrDefault(a => a.Email == email);

            if (admin != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                    return admin;
            }

            return null;
        }

        public Driver RegisterDriver(RegisterDriverDto dto)
        {
            if (_context.Drivers.Any(u => u.Email == dto.Email))
            {
                throw new UserAlreadyExistsException();
            }

            var newDriver = new Driver()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                LicensePlateNumber = dto.LicensePlateNumber,
            };


            var hashedPassword = _passwordHasher.HashPassword(newDriver, dto.Password);
            newDriver.PasswordHash = hashedPassword;
            _context.Drivers.Add(newDriver);
            _context.SaveChanges();
            return newDriver;
        }

        public ParkingAdmin RegisterParkingAdmin(RegisterParkingAdminDto dto)
        {
            if (_context.ParkingAdmins.Any(u => u.Email == dto.Email))
            {
                throw new UserAlreadyExistsException();
            }
            var newParkingAdmin = new ParkingAdmin()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };

            var hashedPassword = _passwordHasher.HashPassword(newParkingAdmin, dto.Password);
            newParkingAdmin.PasswordHash = hashedPassword;
            _context.ParkingAdmins.Add(newParkingAdmin);
            _context.SaveChanges();
            return newParkingAdmin;
        }
    }
}
