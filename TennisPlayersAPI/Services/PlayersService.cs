using TennisPlayersAPI.Exceptions;
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

        /// <summary>
        /// Retourner tous les joueurs de tennis
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Player> GetAllPlayers()
        {
            var players = _repo.GetAll() ?? throw PlayerException.NoPlayersFound();

            if (!players.Any())
                throw PlayerException.NoPlayersFound();

            return players.OrderBy(p => p.Data.Rank);
        }

        /// <summary>
        /// Recherche d'un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id">Identifiant</param>
        /// <returns></returns>
        public Player GetPlayerById(int id)
        {
            var player = _repo.GetById(id) ?? throw PlayerException.NotFound(id);
            return player;
        }

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        public Player AddPlayer(Player player)
        {
            if (_repo.GetAll().Any(p => p.Id == player.Id))
                throw PlayerException.AlreadyExists(player.Id);

            try
            {
                return _repo.Add(player);
            }
            catch (Exception ex)
            {
                throw PlayerException.CreationFailed(ex.Message);
            }
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="id">Identifiant de joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns></returns>
        public Player UpdatePlayer(int id, Player newPlayer)
        {
            var existing =_repo.GetById(id) ?? throw PlayerException.NotFound(id);
            try
            {
                return _repo.Update(id, newPlayer);
            }
            catch (Exception ex)
            {
                throw PlayerException.UpdateFailed(id, ex.Message);
            }
        }

        /// <summary>
        /// Supprimer un joueur de Tennis
        /// </summary>
        /// <param name="id">Identifiant de joueur de tennis</param>
        /// <returns>bool: False ou True</returns>
        public bool DeletePlayer(int id)
        {
            var existing = _repo.GetById(id) ?? throw PlayerException.NotFound(id);
            try
            {
                return _repo.Delete(id);
            }
            catch (Exception ex)
            {
                throw PlayerException.DeletionFailed(id, ex.Message);
            }
        }

        /// <summary>
        /// Statistiques sur les joueurs
        /// - Pays qui a le plus grand ratio de parties gagnées
        /// - IMC moyen de tous les joueurs
        /// - La médiane de la taille des joueurs
        /// </summary>
        /// <returns>Un objet statistique</returns>
        public StatisticsResult GetStats()
        {
            var players = _repo.GetAll().ToList();

            // 1. Pays avec le meilleur ratio de victoires
            var countryStats = players
                .GroupBy(p => p.Country.Code)
                .Select(g => new
                {
                    Country = g.Key,
                    Wins = g.Sum(p => p.Data.Last.Count(l => l == 1)),
                    Total = g.Sum(p => p.Data.Last.Count)
                })
                .Select(x => new
                {
                    x.Country,
                    Ratio = (double)x.Wins / x.Total
                })
                .OrderByDescending(x => x.Ratio)
                .FirstOrDefault();

            string bestCountry = countryStats?.Country ?? "N/A";

            // 2. IMC moyen
            var bmis = players.Select(p =>
            {
                double weightKg = p.Data.Weight / 1000.0;
                double heightM = p.Data.Height / 100.0;
                return weightKg / (heightM * heightM);
            });

            double avgBmi = bmis.Average();

            // 3. Médiane de la taille
            var sortedHeights = players.Select(p => p.Data.Height).OrderBy(h => h).ToList();
            double median;
            int n = sortedHeights.Count;
            if (n % 2 == 1)
                median = sortedHeights[n / 2];
            else
                median = (sortedHeights[(n / 2) - 1] + sortedHeights[n / 2]) / 2.0;

            return new StatisticsResult
            {
                BestCountry = bestCountry,
                AverageBmi = Math.Round(avgBmi, 2),
                MedianHeight = median
            };
        }
    }
}