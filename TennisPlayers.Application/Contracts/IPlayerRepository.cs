
using TennisPlayers.Domain.Entities;

namespace TennisPlayers.Application.Contracts
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Retourner tous les joueurs de tennis
        /// </summary>
        /// <returns>Liste des joueurs</returns>
        Task<IEnumerable<Player>> GetAllAsync();

        /// <summary>
        /// Trouver un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Le joueur trouvé</returns>
        Task<Player?> GetByIdAsync(int id);

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns>Le joueur ajouté</returns>
        Task<Player> AddAsync(Player player);

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns>Le joueur modifié</returns>
        Task<Player> UpdateAsync(Player player);

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        Task DeleteAsync(int id);

    }
}