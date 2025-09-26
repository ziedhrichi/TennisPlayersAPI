
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TennisPlayersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        [HttpGet("hello")]
        [Authorize]
        public IActionResult GetHello()
        {
            return Ok(new { message = "Ceci est protégé 🚀", user = User.Identity?.Name ?? "inconnu" });
        }
    }
}
