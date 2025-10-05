using System.Text.Json;
using TennisPlayers.Domain.Entities;


namespace TennisPlayers.Infrastructure.Persistence
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
        public Task<IEnumerable<Player>> GetAllAsync() => Task.FromResult(_root.Players.AsEnumerable());


        /// <summary>
        /// Trouver un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Le joueur trouvé</returns>
        public Task<Player?> GetByIdAsync(int id) => Task.FromResult(_root.Players.FirstOrDefault(p => p.Id == id));

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns>Le joueur ajouté</returns>
        public Task<Player> AddAsync(Player player)
        {
            player.Id = _root.Players.Max(p => p.Id) + 1;
            _root.Players.Add(player);
            return Task.FromResult(player);
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns>Le joueur modifié</returns>
        public Task<Player> UpdateAsync(Player player)
        {
            var existing = _root.Players.FirstOrDefault(p => p.Id == player.Id);
            if (existing is null) throw new KeyNotFoundException("Player not found");
            _root.Players.Remove(existing);
            _root.Players.Add(player);
            return Task.FromResult(player);
        }

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        public Task DeleteAsync(int id)
        {
            var player = _root.Players.FirstOrDefault(p => p.Id == id);
            if (player is not null) _root.Players.Remove(player);
            return Task.CompletedTask;
        }
      
    }

}