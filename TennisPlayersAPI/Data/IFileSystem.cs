namespace TennisPlayersAPI.Data
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }

}
