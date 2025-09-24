using Microsoft.AspNetCore.Mvc;
using TennisPlayersAPI.Services;

namespace TennisPlayersAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TennisPlayersController : ControllerBase
    {
        private readonly IPlayersService _service;

        public TennisPlayersController(IPlayersService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetPlayers()
        {
            return Ok(_service.GetTopPlayers(10));
        }

        [HttpGet("{id}")]
        public IActionResult GetPlayer(int id)
        {
            var player = _service.GetPlayerById(id);
            if (player == null) return NotFound();
            return Ok(player);
        }

    }
}