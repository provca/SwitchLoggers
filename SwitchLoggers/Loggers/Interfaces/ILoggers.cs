namespace SwitchLoggers.Loggers.Interfaces
{
    /// <summary>
    /// Interface for logging operations.
    /// </summary>
    public interface ILoggers
    {
        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The exception message to log.</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The exception message to log.</param>
        void LogError(string message);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The exception message to log.</param>
        void LogFatal(string message);

        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The information message to log.</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs a verbose message.
        /// </summary>
        /// <param name="message">The exception message to log.</param>
        void LogVerbose(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);
    }
}
