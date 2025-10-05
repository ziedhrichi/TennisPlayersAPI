using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TennisPlayers.Application.Contracts;
using TennisPlayers.Domain.Entities;

namespace TennisPlayersAPI.Controllers
{
    /// <summary>
    /// Class controlleur de joueur de tennis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TennisPlayersController : ControllerBase
    {
        private readonly IPlayersService _service;
        private readonly ILogger<TennisPlayersController> _logger;

        public TennisPlayersController(IPlayersService service, ILogger<TennisPlayersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Liste des joueurs de Tennis triée du meilleur au moins bon.
        /// </summary>
        /// <returns>Liste des joueurs de Tennis</returns>
        [HttpGet]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetPlayers()
        {
            return Ok(await _service.GetPlayersAsync());
        }

        /// <summary>
        /// Les informations d'un joueur de tennis gràce à son ID
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>Joueur de tennis</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            _logger.LogInformation("Recherche du joueur avec ID {PlayerId}", id);

            var player = await _service.GetPlayerByIdAsync(id);
            if (player == null) return NotFound();

            _logger.LogInformation("Joueur trouvé : {@Player}", player);
            return Ok(player);
        }

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Write")]
        public async Task<IActionResult> AddPlayer([FromBody] Player player)
        {
            _logger.LogInformation("Ajouter un nouveau joueur avec ID {PlayerId}", player.Id);

            var created = await _service.AddPlayerAsync(player);

            _logger.LogInformation("Joueur ajouté : {@Player}", player);

            return CreatedAtAction(nameof(GetPlayer), new { id = created.Id }, created);
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="player">le nouveau joueur rempplacé</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "Write")]
        public async Task<IActionResult> UpdatePlayer([FromBody] Player player)
        {
            _logger.LogInformation("Modifier le joueur avec ID {PlayerId}", player.Id);

            var updated = await _service.UpdatePlayerAsync(player);
            if (updated == null) return NotFound();

            _logger.LogInformation("Joueur modifié : {@Player}", player);

            return Ok(updated);
        }

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            _logger.LogInformation("Supprimer le joueur avec ID {PlayerId}", id);

             await _service.DeletePlayerAsync(id);

            _logger.LogInformation("Le joueur avec l'id {PlayerId} a été supprimé ", id);

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
        [Authorize(Policy = "Read")]
        public IActionResult GetStatistics()
        {
            var stats = _service.GetStats();
            return Ok(stats);
        }

        public record ApiErrorResponse(string Error, string Message, int? PlayerId);

    }
}