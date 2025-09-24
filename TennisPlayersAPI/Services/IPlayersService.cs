using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Services
{
    public interface IPlayersService
    {
        /// <summary>
        /// Retourner tous les joueurs de tennis
        /// </summary>
        /// <returns></returns>
        IEnumerable<Player> GetAllPlayers();

        /// <summary>
        /// Recherche d'un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id">Identifiant</param>
        /// <returns></returns>
        Player GetPlayerById(int id);

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        Player AddPlayer(Player player);

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns></returns>
        Player UpdatePlayer(int id, Player newPlayer);

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        bool DeletePlayer(int id);

        /// <summary>
        /// Statistiques sur les joueurs
        /// - Pays qui a le plus grand ratio de parties gagnées
        /// - IMC moyen de tous les joueurs
        /// - La médiane de la taille des joueurs
        /// </summary>
        /// <returns>Un objet statistique</returns>
        StatisticsResult GetStats();
    }
}