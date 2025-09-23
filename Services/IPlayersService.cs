using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Services
{
    public interface IPlayersService
    {
        IEnumerable<Player> GetTopPlayers(int n);

        Player GetPlayerById(int id);
    }
}