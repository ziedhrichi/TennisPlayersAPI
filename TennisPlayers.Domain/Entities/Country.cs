namespace TennisPlayers.Domain.Entities
{
    public class Country
    {
        /// <summary>
        /// Photo de pays
        /// </summary>
        public string? Picture { get; set; }

        /// <summary>
        /// Code de pays
        /// </summary>
        public required string Code { get; set; }
    }
}
