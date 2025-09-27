namespace TennisPlayersAPI.Data
{
    /// <summary>
    /// Interface pour gérer la lecture et l'ecriture dans un json file
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Lecture d'un json file
        /// </summary>
        /// <param name="path">lien vers le fichier</param>
        /// <returns></returns>
        string ReadAllText(string path);

        /// <summary>
        /// Ecriture dans json file
        /// </summary>
        /// <param name="path">lien vers le fichier</param>
        /// <param name="contents">Les données à ajouter</param>
        void WriteAllText(string path, string contents);
    }

}
