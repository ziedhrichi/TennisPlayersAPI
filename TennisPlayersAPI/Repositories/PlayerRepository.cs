using System.Text.Json;
using TennisPlayersAPI.Data;
using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Repositories
{
    /// <summary>
    /// Classe est chargé d’accéder aux données des joueurs.
    /// </summary>
    internal class PlayerRepository : IPlayerRepository
    {
        private readonly Root _root;
        private readonly IFileSystem _fileSystem;
        private readonly string _filePath = "Data/Players.json";

        public PlayerRepository(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            string json = _fileSystem.ReadAllText(_filePath);
            _root = JsonSerializer.Deserialize<Root>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        /// <summary>
        /// Retourner tous les joueurs de tennis
        /// </summary>
        /// <returns>Liste des joueurs</returns>
        public IEnumerable<Player> GetAll() => _root.Players;

        /// <summary>
        /// Trouver un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Le joueur trouvé</returns>
        public Player GetById(int id) => _root.Players.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns>Le joueur ajouté</returns>
        public Player Add(Player player)
        {
            player.Id = _root.Players.Max(p => p.Id) + 1;
            _root.Players.Add(player);
            Save();
            return player;
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns>Le joueur modifié</returns>
        public Player Update(int id, Player newPlayer)
        {
            var player = _root.Players.FirstOrDefault(p => p.Id == id);
            if (player == null) return null;

            newPlayer.Id = id;
            _root.Players[_root.Players.IndexOf(player)] = newPlayer;
            Save();
            return newPlayer;
        }

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        public bool Delete(int id)
        {
            var player = _root.Players.FirstOrDefault(p => p.Id == id);
            if (player == null) return false;

            _root.Players.Remove(player);
            Save();
            return true;
        }
        /// <summary>
        /// Une méthode privée pour gérer le sauvegarde de fichier
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        private void Save()
        {
            string json = JsonSerializer.Serialize(_root, new JsonSerializerOptions { WriteIndented = true });
            _fileSystem.WriteAllText(_filePath, json);
        }
    }

}