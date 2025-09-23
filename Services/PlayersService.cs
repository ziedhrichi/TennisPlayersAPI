using TennisPlayersAPI.Models;
using TennisPlayersAPI.Repositories;

namespace TennisPlayersAPI.Services
{
    internal class PlayersService : IPlayersService
    {
        private readonly IPlayerRepository _repo;

        public PlayersService(IPlayerRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Player> GetTopPlayers(int n)
        {
            return _repo.GetAll()
                        .OrderBy(p => p.Data.Rank)
                        .Take(n);
        }

        public Player GetPlayerById(int id) => _repo.GetById(id);
    }
}