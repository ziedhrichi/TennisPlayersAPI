using TennisPlayersAPI.Models;

namespace TennisPlayersAPI.Repositories
{
    internal interface IPlayerRepository
    {
        IEnumerable<Player> GetAll();

        Player GetById(int id);

    }
}