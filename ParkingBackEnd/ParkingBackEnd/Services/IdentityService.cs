using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingBackEnd.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingBackEnd.Services
{
    public interface IIdentityService
    {
        public string? GenerateToken(TokenClaims request);
    }

    public class IdentityService : IIdentityService
    {
        private const string TokenSecret = "secret4w78efhc2783gd671872e2@!WDX!@#!~!@$!@E@!1wd12";
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromDays(100);

        public string? GenerateToken(TokenClaims request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(TokenSecret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.Email),
                new(JwtRegisteredClaimNames.Email, request.Email),
                new("Email", request.Email),
                new("UserId", request.UserId.ToString()),
                new("LoginType", request.LoginType)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = "https://Parking",
                Audience = "https://localhost:7026/api/",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

    }
}
