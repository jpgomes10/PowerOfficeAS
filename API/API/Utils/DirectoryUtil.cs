namespace API.Utils
{
    public class DirectoryUtil
    {
        private static readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Creates a new directory with the specified folder name under the base path, and returns a file path with the specified extension that includes a timestamp.
        /// If the directory already exists, it will not be created again.
        /// </summary>
        /// <param name="folderName">The name of the folder to create.</param>
        /// <param name="extension">The extension of the file to create.</param>
        /// <returns>
        ///     A file path with the specified extension that includes a timestamp.
        ///     Success: The file path of the created file.
        ///     Failure: An empty string if the folder could not be created or the file path could not be generated.
        /// </returns>
        public static string CreateOutputFileWithTimestamp(string folderName, string extension)
        {
            string outputFolderPath = Path.Combine(_basePath, folderName);
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            var logFileName = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ") + "." + extension;
            return Path.Combine(outputFolderPath, logFileName);
        }
    }
}
