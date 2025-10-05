using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TennisPlayers.Domain.Entities;

namespace TennisPlayers.API.Controllers
{
    /// <summary>
    /// Class controlleur pour gérer les roles des utilisateurs
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Fonction pour génerer un token pu un role donné
        /// </summary>
        /// <param name="request">la requette sous forme d'un json file avec nom d'utilisateur et mot de passe</param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string role;

            if (request.Username == "admin" && request.Password == "1234")
            {
                role = "Admin";
            }
            else if (request.Username == "editor" && request.Password == "1234")
            {
                role = "Editor";
            }
            else if (request.Username == "visitor" && request.Password == "1234")
            {
                role = "Visitor";
            }
            else
            {
                return Unauthorized();
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
        new Claim("role", role)
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

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
