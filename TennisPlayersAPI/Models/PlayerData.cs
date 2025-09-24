namespace TennisPlayersAPI.Models
{
    public class PlayerData
    {
        /// <summary>
        /// Rang de joueur
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Les points de joueur
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Poid de joueur
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Taille de joueur
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Age de joueur
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Derniére statitstique de joueur
        /// </summary>
        public List<int>? Last { get; set; }

    }
}
