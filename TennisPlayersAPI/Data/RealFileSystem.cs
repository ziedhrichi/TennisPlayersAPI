namespace TennisPlayersAPI.Data
{
    internal class RealFileSystem : IFileSystem
    {
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
    }

}
