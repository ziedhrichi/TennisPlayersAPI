using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Repositories
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Retourner tous les joueurs de tennis
        /// </summary>
        /// <returns></returns>
        IEnumerable<Player> GetAll();

        /// <summary>
        /// Trouver un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Player GetById(int id);

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        Player Add(Player player);

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns></returns>
        Player Update(int id, Player newPlayer);

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        bool Delete(int id);

    }
}