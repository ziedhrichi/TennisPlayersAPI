namespace TennisPlayersAPI.Models
{
    public class StatisticsResult
    {
        /// <summary>
        /// Pays qui a le plus grand ratio de parties gagnées
        /// </summary>
        public string BestCountry { get; set; }

        /// <summary>
        /// IMC moyen de tous les joueurs
        /// </summary>
        public double AverageBmi { get; set; }

        /// <summary>
        /// La médiane de la taille des joueurs
        /// </summary>
        public double MedianHeight { get; set; }

    }
}
