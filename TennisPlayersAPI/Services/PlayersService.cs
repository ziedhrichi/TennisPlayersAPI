using System.Numerics;
using TennisPlayersAPI.Exceptions;
using TennisPlayersAPI.Models;
using TennisPlayersAPI.Repositories;

namespace TennisPlayersAPI.Services
{
    /// <summary>
    /// Service applicatif chargé de la gestion et du traitement des joueurs.
    /// Cette classe implémente IPlayersService et encapsule 
    /// la logique métier liée aux joueurs
    /// </summary>
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
        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            var players = await _repo.GetAllAsync() ?? throw PlayerException.NoPlayersFound();

            if (!players.Any())
                throw PlayerException.NoPlayersFound();

            return players.OrderBy(p => p.Data.Rank);
        }

        /// <summary>
        /// Recherche d'un joueur de tennis par son identifiant
        /// </summary>
        /// <param name="id">Identifiant</param>
        /// <returns></returns>
        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            var player =  await _repo.GetByIdAsync(id) ?? throw PlayerException.NotFound(id);
            return player;
        }

        /// <summary>
        /// Ajouter un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Objet joueur</param>
        /// <returns></returns>
        public async Task<Player> AddPlayerAsync(Player player)
        {
            var _player = GetPlayerByIdAsync(player.Id);
            if (!_player.IsFaulted)
                throw PlayerException.AlreadyExists(player.Id);

            try
            {
                return await _repo.AddAsync(player);
            }
            catch (Exception ex)
            {
                throw PlayerException.CreationFailed(ex.Message);
            }
        }

        /// <summary>
        /// Modifier un joueur de tennis dans la liste des joueurs
        /// </summary>
        /// <param name="player">Le joueur à modifier</param>
        /// <param name="newPlayer">le nouveau joueur rempplacé</param>
        /// <returns></returns>

        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            if (GetPlayerByIdAsync(player.Id) != null)  throw PlayerException.NotFound(player.Id);
            try
            {
                return await _repo.UpdateAsync(player);

            }
            catch (Exception ex)
            {
                throw PlayerException.UpdateFailed(player.Id, ex.Message);
            }
        }

        public async Task DeletePlayerAsync(int id)
        {
            if (GetPlayerByIdAsync(id) != null) throw PlayerException.NotFound(id);

            try
            {
                await _repo.DeleteAsync(id);
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
            try
            {
                var players = GetPlayersAsync().Result; //Au début j'ai fait déclarer la méthode avec List au lieu de IEnumerable et j'ai oublié de supprimer .tolist()

                if (players == null || !players.Any())
                    throw PlayerException.NoPlayersFound();

                return new StatisticsResult
                {
                    BestCountry = CalculateBestCountry(players),
                    AverageBmi = CalculateAverageBmi(players),
                    MedianHeight = CalculateMedianHeight(players)
                };
            }
            catch (PlayerException)
            {
                // On relance les exceptions métier prévues
                throw;
            }
            catch (Exception ex)
            {
                // Toute autre erreur est encapsulée
                throw PlayerException.UpdateFailed(0, $"Erreur lors du calcul des statistiques : {ex.Message}");
            }
        }

       

        /// <summary>
        /// Calcule le pays avec le meilleur ratio de victoires parmi les joueurs.
        /// </summary>
        /// <param name="players">Liste des joueurs à analyser.</param>
        /// <returns>
        /// Le code du pays avec le meilleur ratio de victoires, ou "N/A" si aucun pays n'est trouvé.
        /// </returns>
        private string CalculateBestCountry(IEnumerable<Player> players)
        {
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
                    Ratio = x.Total > 0 ? (double)x.Wins / x.Total : 0
                })
                .OrderByDescending(x => x.Ratio)
                .FirstOrDefault();

            return countryStats?.Country ?? "N/A";
        }

        /// Calcule l'IMC moyen de l'ensemble des joueurs.
        /// </summary>
        /// <param name="players">Liste des joueurs à analyser.</param>
        /// <returns>
        /// L'IMC moyen, arrondi à deux décimales.
        /// </returns>
        /// <exception cref="PlayerException">
        /// Lancée si la taille d'un joueur est invalide (inférieure ou égale à zéro).
        /// </exception>
        private double CalculateAverageBmi(IEnumerable<Player> players)
        {
            var bmis = players.Select(p =>
            {
                double weightKg = p.Data.Weight / 1000.0; // grammes → kilogrammes
                double heightM = p.Data.Height / 100.0; // cm → mètres

                if (heightM <= 0)
                    throw PlayerException.UpdateFailed(p.Id, "Taille invalide pour le calcul de l'IMC.");

                return weightKg / (heightM * heightM);
            });

            return Math.Round(bmis.Average(), 2);
        }

        /// <summary>
        /// Calcule la médiane des tailles des joueurs.
        /// </summary>
        /// <param name="players">Liste des joueurs à analyser.</param>
        /// <returns>
        /// La taille médiane des joueurs.
        /// </returns>
        /// <exception cref="PlayerException">
        /// Lancée si aucun joueur n'est disponible.
        /// </exception>
        private double CalculateMedianHeight(IEnumerable<Player> players)
        {
            var sortedHeights = players.Select(p => p.Data.Height).OrderBy(h => h).ToList();

            if (!sortedHeights.Any())
                throw PlayerException.NoPlayersFound();

            int n = sortedHeights.Count;

            return (n % 2 == 1)
                ? sortedHeights[n / 2]
                : (sortedHeights[(n / 2) - 1] + sortedHeights[n / 2]) / 2.0;
        }
    }
}