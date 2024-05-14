using SwitchLoggers.Settings;
using System.Runtime.InteropServices;

namespace SwitchLoggers.Utilities
{
    /// <summary>
    /// Utility class for loggers.
    /// </summary>
    public class Utilities_Loggers
    {
        /// <summary>
        /// Sets the complete path for logging files.
        /// </summary>
        /// <param name="loggerName">The name of the logger.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        /// <returns>A tuple containing the complete file path and file name.</returns>
        public static (string FilePath, string FileName) SetCompletePath(string loggerName, string filePath, string fileName)
        {
            // Set a default path if it's empty or contains invalid characters.
            if (string.IsNullOrEmpty(filePath) || filePath.Any(c => Path.GetInvalidPathChars().Contains(c)))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "_loggers", loggerName);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "_loggers", loggerName);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "_loggers", loggerName);
                }
            }

            // Set a default file name if it's empty or contains invalid characters.
            if (string.IsNullOrEmpty(fileName) || fileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                fileName = $"{loggerName}.txt";
            }

            // Update Settings.
            SwitchLoggersSettings.FilePath = filePath;
            SwitchLoggersSettings.FileName = fileName;

            // Return checked and/or updated values.
            return (filePath, fileName);
        }
    }
}
