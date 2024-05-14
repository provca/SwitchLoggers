namespace SwitchLoggers.Loggers.Interfaces
{
    /// <summary>
    /// Interface for the factory responsible for creating loggers.
    /// </summary>
    public interface IFactory_Loggers
    {
        /// <summary>
        /// Creates a logger instance based on the specified parameters.
        /// </summary>
        /// <param name="logName">The name of the logger.</param>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored (optional).</param>
        /// <param name="fileName">The name of the log file (optional).</param>
        /// <returns>An instance of the created logger.</returns>
        ILoggers CreateLogger(string logName, bool enableFileLogging, string filePath = "", string fileName = "");
    }
}
