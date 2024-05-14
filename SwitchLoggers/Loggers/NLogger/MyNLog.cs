using NLog;
using NLog.Targets;
using SwitchLoggers.Enums;
using SwitchLoggers.Loggers.Interfaces;
using SwitchLoggers.Settings;
using SwitchLoggers.Utilities;

namespace SwitchLoggers.Loggers.NLogger
{
    /// <summary>
    /// Represents the NLog logger implementation.
    /// </summary>
    public class MyNLog : ILoggers, IFactory_Loggers
    {
        private readonly Logger logger;

        /// <summary>
        /// Constructor for MyNLog.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        public MyNLog(bool enableFileLogging, string filePath, string fileName)
        {
            // Build the NLog logger during initialization.
            logger = BuildNLog(enableFileLogging, filePath, fileName) ?? throw new ArgumentNullException(nameof(logger));

            // Check if logger has been enabled to save data in a file.
            if (!enableFileLogging)
                logger.Warn($"Any file has been defined to save logs yet.");
        }

        /// <inheritdoc/>
        public ILoggers CreateLogger(string logName, bool enableFileLogging, string filePath, string fileName)
        {
            return this;
        }

        /// <inheritdoc/>
        public void LogDebug(string message)
        {
            try
            {
                logger.Debug(message);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"NLog Debug Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogError(string message)
        {
            try
            {
                logger.Error(message);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"NLog Error Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogFatal(string message)
        {
            try
            {
                logger.Fatal(message);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"NLog Fatal Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogInformation(string message)
        {
            try
            {
                logger.Info(message);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"NLog Information Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogVerbose(string message)
        {
            try
            {
                logger.Trace(message);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"NLog Verbose Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogWarning(string message)
        {
            try
            {
                logger.Warn(message);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"NLog Warning Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <summary>
        /// Builds an NLog logger based on the specified configuration.
        /// Visit <see cref="Utilities_Loggers"/> class.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        /// <returns>The configured NLog logger.</returns>
        private static Logger BuildNLog(bool enableFileLogging, string filePath, string fileName)
        {
            // Create a new logging configuration
            var config = new NLog.Config.LoggingConfiguration();

            // Create a console target with the specified log level
            var consoleLogLevelRule = NLog.LogLevel.FromString(SwitchLoggersSettings.ConsoleNLogLevel.ToString());
            var consoleTarget = new ConsoleTarget("console");
            config.AddRule(consoleLogLevelRule, NLog.LogLevel.Fatal, consoleTarget);

            if (enableFileLogging)
            {
                // Set a default path if it's empty or contains invalid characters
                var (path, file) = Utilities_Loggers.SetCompletePath(nameof(LoggerType.NLog), filePath, fileName);

                // Create a file target with the specified file path and file name
                var fileTarget = new FileTarget("file")
                {
                    FileName = Path.Combine(@path, @file),
                    KeepFileOpen = false
                };

                // Add a rule to log to the file target with the specified log level
                var fileLogLevelRule = NLog.LogLevel.FromString(SwitchLoggersSettings.FileNLogLevel.ToString());
                config.AddRule(fileLogLevelRule, NLog.LogLevel.Fatal, fileTarget);
            }

            // Apply the logging configuration
            NLog.LogManager.Configuration = config;

            // Create and return the logger
            var logger = NLog.LogManager.GetLogger("NLog");
            return logger;
        }
    }
}
