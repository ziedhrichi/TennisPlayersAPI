using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Sdk;

namespace TennisPlayers.Domain.Entities
{
    public class PlayerData
    {
        /// <summary>
        /// Rang de joueur
        /// </summary>
        [Required]
        public int Rank { get; set; }

        /// <summary>
        /// Les points de joueur
        /// </summary>
        [Required]
        public int Points { get; set; }

        /// <summary>
        /// Poid de joueur
        /// </summary>
        [Range(30, 200, ErrorMessage = "Le poids doit être compris entre 30kg et 200kg.")]
        public int Weight { get; set; }

        /// <summary>
        /// Taille de joueur
        /// </summary>
        [Range(50, 250, ErrorMessage = "La taille doit être comprise entre 50cm et 250cm.")]
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
