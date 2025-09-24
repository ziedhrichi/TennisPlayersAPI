using System.Text.Json;
using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Repositories
{
    internal class PlayerRepository :IPlayerRepository
    {
        private readonly Root _root;

        public PlayerRepository()
        {
            // Ici : lecture JSON (ou connexion DB)
            string json = File.ReadAllText("Data/Players.json");
            _root = JsonSerializer.Deserialize<Root>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        /// <summary>
        /// Retourner tous les joueurs de tennis
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Player> GetAll() => _root.Players;

        /// <summary>
        /// Trouver un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Player GetById(int id) => _root.Players.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        public Player Add(Player player)
        {
            player.Id = _root.Players.Max(p => p.Id) + 1; // auto incrément simple
            _root.Players.Add(player);
            Save();
            return player;
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns></returns>
        public Player Update(int id, Player newPlayer)
        {
            var player = _root.Players?.FirstOrDefault(p => p.Id == id);
            if (player == null) return null;

            // remplacement complet
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
        /// Sauvegarder les données dans json file
        /// </summary>
        private void Save()
        {
            string json = JsonSerializer.Serialize(_root, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("Data/Players.json", json);
        }
    }
}