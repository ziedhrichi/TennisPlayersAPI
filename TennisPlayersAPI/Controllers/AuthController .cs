using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string role;

            // 🎯 Vérification simple (exemple en dur)
            if (request.Username == "admin" && request.Password == "1234")
            {
                role = "Admin";
            }
            else if (request.Username == "editor" && request.Password == "1234")
            {
                role = "Editor";
            }
            else if (request.Username == "user" && request.Password == "1234")
            {
                role = "User";
            }
            else
            {
                return Unauthorized();
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
        new Claim(ClaimTypes.Role, role)
    };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), role });
        }
    }
}
