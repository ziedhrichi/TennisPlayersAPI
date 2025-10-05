namespace TennisPlayers.Domain.Exceptions
{
    public enum PlayerErrorType
    {
        NotFound,
        AlreadyExists,
        CreationFailed,
        UpdateFailed,
        DeletionFailed,
        Forbidden
    }
}
