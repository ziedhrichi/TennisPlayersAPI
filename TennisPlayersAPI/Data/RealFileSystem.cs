namespace TennisPlayersAPI.Data
{
    internal class RealFileSystem : IFileSystem
    {
        /// <summary>
        /// Lecture d'un json file
        /// </summary>
        /// <param name="path">lien vers le fichier</param>
        /// <returns></returns>
        public string ReadAllText(string path) => File.ReadAllText(path);

        /// <summary>
        /// Ecriture dans json file
        /// </summary>
        /// <param name="path">lien vers le fichier</param>
        /// <param name="contents">Les données à ajouter</param>
        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
    }

}
