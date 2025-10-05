using static TennisPlayersAPI.Controllers.TennisPlayersController;

namespace TennisPlayersAPI.Exceptions
{
    public class PlayerException : Exception
    {
        public PlayerErrorType ErrorType { get; }

        public int? PlayerId { get; }

        public PlayerException(PlayerErrorType errorType, string message, int? playerId = null)
            : base(message)
        {
            ErrorType = errorType;
            PlayerId = playerId;
        }

        // Méthodes factory pour simplifier l'usage
        public static PlayerException NotFound(int id) =>
            new(PlayerErrorType.NotFound, $"Le joueur avec l'ID {id} est introuvable.", id);

        public static PlayerException AlreadyExists(int id) =>
            new(PlayerErrorType.AlreadyExists, $"Un joueur avec l'ID {id} existe déjà.", id);

        public static PlayerException CreationFailed(string reason) =>
            new(PlayerErrorType.CreationFailed, $"Erreur lors de la création du joueur : {reason}");

        public static PlayerException UpdateFailed(int id, string reason) =>
            new(PlayerErrorType.UpdateFailed, $"Impossible de mettre à jour le joueur {id} : {reason}", id);

        public static PlayerException DeletionFailed(int id, string reason) =>
            new(PlayerErrorType.DeletionFailed, $"Impossible de supprimer le joueur {id} : {reason}", id);

        public static PlayerException NoPlayersFound() =>
            new(PlayerErrorType.NotFound, "Aucun joueur n’a pu être chargé depuis la source de données.");

        //En supprimant ce contrôle, aucun message explicite n’informera l’utilisateur lorsqu’il n’a pas les droits nécessaires.
        public static PlayerException AccessDenied() =>
            new(PlayerErrorType.Forbidden, "Accès refusé : vous n’avez pas les droits suffisants pour effectuer cette action.");
    }

}
