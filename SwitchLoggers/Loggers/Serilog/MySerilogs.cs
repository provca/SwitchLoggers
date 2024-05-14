using Serilog;
using Serilog.Core;
using Serilog.Events;
using SwitchLoggers.Enums;
using SwitchLoggers.Loggers.Interfaces;
using SwitchLoggers.Utilities;

namespace SwitchLoggers.Loggers.Serilogs
{
    /// <summary>
    /// Implementation of the <see cref="ILoggers"/> and <see cref="IFactory_Loggers"/> interfaces using Serilog.
    /// </summary>
    public class MySerilogs : ILoggers, IFactory_Loggers
    {
        private readonly Logger logger;

        /// <summary>
        /// Constructor for MySerilogs.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        public MySerilogs(bool enableFileLogging, string filePath, string fileName)
        {
            // Build the Serilog logger during initialization.
            logger = BuildSerilog(enableFileLogging, filePath, fileName) ?? throw new ArgumentNullException(nameof(logger));

            // Check if logger has been enabled to save data in a file.
            if (!enableFileLogging)
                logger.Warning($"Any file has been defined to save logs yet.");
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
                logger.Debug($"{DateTime.Now:yyyy-MM-dd} {message}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"Serilog Debug Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogError(string message)
        {
            try
            {
                logger.Error($"{DateTime.Now:yyyy-MM-dd} {message}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"Serilog Error Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogFatal(string message)
        {
            try
            {
                logger.Fatal($"{DateTime.Now:yyyy-MM-dd} {message}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"Serilog Fatal Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogInformation(string message)
        {
            try
            {
                logger.Information($"{DateTime.Now:yyyy-MM-dd} {message}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"Serilog Information Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogVerbose(string message)
        {
            try
            {
                logger.Verbose($"{DateTime.Now:yyyy-MM-dd} {message}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"Serilog Verbose Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void LogWarning(string message)
        {
            try
            {
                logger.Warning($"{DateTime.Now:yyyy-MM-dd} {message}");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during logging.
                throw new Exception($"Serilog Warning Exception Type: {ex.GetType().Name}\n{ex.Message}");
            }
        }

        /// <summary>
        /// Builds a Serilog logger based on the specified configuration.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        /// <returns>The configured Serilog logger.</returns>
        private static Logger BuildSerilog(bool enableFileLogging, string filePath, string fileName)
        {
            // Create a new logger configuration
            var loggerConfiguration = new LoggerConfiguration();

            // Configure console logging with the specified level
            loggerConfiguration.WriteTo.Console();

            if (enableFileLogging)
            {
                // Set a default path if it's empty or contains invalid characters
                var (path, file) = Utilities_Loggers.SetCompletePath(nameof(LoggerType.Serilog), filePath, fileName);

                // Combine the directory path and file name to get the full file path
                string fullFilePath = Path.Combine(@path, @file);

                // Configure file logging with the specified level
                loggerConfiguration.WriteTo.File(
                    fullFilePath,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: null,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1)
                );
            }

            // Create and configure the logger
            var logger = loggerConfiguration.CreateLogger();

            // Set the logger as the default logger for the application
            Log.Logger = logger;

            return logger;
        }
    }
}
