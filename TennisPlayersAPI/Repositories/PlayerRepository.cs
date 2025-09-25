using System.Text.Json;
using TennisPlayersAPI.Data;
using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Repositories
{
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

        public IEnumerable<Player> GetAll() => _root.Players;

        public Player GetById(int id) => _root.Players.FirstOrDefault(p => p.Id == id);

        public Player Add(Player player)
        {
            player.Id = _root.Players.Max(p => p.Id) + 1;
            _root.Players.Add(player);
            Save();
            return player;
        }

        public Player Update(int id, Player newPlayer)
        {
            var player = _root.Players.FirstOrDefault(p => p.Id == id);
            if (player == null) return null;

            newPlayer.Id = id;
            _root.Players[_root.Players.IndexOf(player)] = newPlayer;
            Save();
            return newPlayer;
        }

        public bool Delete(int id)
        {
            var player = _root.Players.FirstOrDefault(p => p.Id == id);
            if (player == null) return false;

            _root.Players.Remove(player);
            Save();
            return true;
        }

        private void Save()
        {
            string json = JsonSerializer.Serialize(_root, new JsonSerializerOptions { WriteIndented = true });
            _fileSystem.WriteAllText(_filePath, json);
        }
    }

}