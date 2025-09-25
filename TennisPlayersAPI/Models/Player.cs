namespace TennisPlayersAPI.Models
{
    /// <summary>
    /// Class Player
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Identifiant de joueur
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Prénom de joueur
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Nom de joueur
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Nom utilisé de joueur
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// Sexe de joueur
        /// </summary>
        public required string Sex { get; set; }

        /// <summary>
        /// Pays de joueur
        /// </summary>
        public required Country Country { get; set; }

        /// <summary>
        /// Photo de joueur
        /// </summary>
        public string? Picture { get; set; }

        /// <summary>
        /// Plus de données sur le joueur
        /// </summary>
        public required PlayerData Data { get; set; }

    }

    public class Root
    {
        public List<Player> Players { get; set; }
    }
}
