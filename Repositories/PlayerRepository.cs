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

        public IEnumerable<Player> GetAll() => _root.Players;

        public Player GetById(int id) => _root.Players.FirstOrDefault(p => p.Id == id);
    }
}