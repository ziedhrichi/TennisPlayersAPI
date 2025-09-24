using Microsoft.AspNetCore.Mvc;
using TennisPlayersAPI.Models;
using TennisPlayersAPI.Services;

namespace TennisPlayersAPI.Controllers
{
    /// <summary>
    /// Class controlleur de joueur de tennis
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TennisPlayersController : ControllerBase
    {
        private readonly IPlayersService _service;

        public TennisPlayersController(IPlayersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Liste des joueurs de Tennis triée du meilleur au moins bon.
        /// </summary>
        /// <returns>Liste des joueurs de Tennis</returns>
        [HttpGet]
        public IActionResult GetPlayers()
        {
            return Ok(_service.GetAllPlayers());
        }

        /// <summary>
        /// Les informations d'un joueur de tennis gràce à son ID
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>Joueur de tennis</returns>
        [HttpGet("{id}")]
        public IActionResult GetPlayer(int id)
        {
            var player = _service.GetPlayerById(id);
            if (player == null) return NotFound();
            return Ok(player);
        }

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPlayer([FromBody] Player player)
        {
            var created = _service.AddPlayer(player);
            return CreatedAtAction(nameof(GetPlayer), new { id = created.Id }, created);
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="player">le nouveau joueur rempplacé</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, [FromBody] Player player)
        {
            var updated = _service.UpdatePlayer(id, player);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var deleted = _service.DeletePlayer(id);
            if (!deleted) return NotFound();
            return NoContent(); 
        }

        /// <summary>
        /// Statistiques sur les joueurs
        /// - Pays qui a le plus grand ratio de parties gagnées
        /// - IMC moyen de tous les joueurs
        /// - La médiane de la taille des joueurs
        /// </summary>
        /// <returns>Un objet statistique</returns>
        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var stats = _service.GetStats();
            return Ok(stats);
        }

    }
}