using SwitchLoggers.Enums;
using SwitchLoggers.Loggers.Interfaces;
using SwitchLoggers.Loggers.NLogger;
using SwitchLoggers.Loggers.Serilogs;
using SwitchLoggers.Loggers.TraceLogger;
using SwitchLoggers.Settings;
using System.Diagnostics;

namespace SwitchLoggers.Loggers
{
    /// <summary>
    /// Factory class for creating instances of loggers implementing the <see cref="ILoggers"/> interface.
    /// Also visit <see cref="IFactory_Loggers"/> interface.
    /// </summary>
    public class Factory_Loggers : IFactory_Loggers
    {
        /// <inheritdoc/>
        public ILoggers CreateLogger(string logName, bool enableFileLogging, string filePath = "", string fileName = "")
        {
            try
            {
                // Check if the log name exists.
                if (!SwitchLoggersSettings.ListOfLoggers.Contains(logName))
                {
                    // If log name is invalid, update to Trace logger.
                    logName = string.IsNullOrEmpty(logName) ? "empty logName" : logName;
                    Trace.WriteLine($"[WARNING {SwitchLoggersSettings.DateTimeNow} ]: A valid logger is nedeed and the following log does not exist: '{logName}'");

                    // Set Trace as a default logger.
                    Trace.WriteLine($"[INFO    {SwitchLoggersSettings.DateTimeNow} ]: Setting as a default logger: {logName = nameof(LoggerType.TraceLog)}");
                }

                // Choose logger implementation based on the specified logger type.
                return logName switch
                {
                    nameof(LoggerType.NLog) => CreateNLog(enableFileLogging, filePath, fileName),
                    nameof(LoggerType.Serilog) => CreateSerilog(enableFileLogging, filePath, fileName),
                    nameof(LoggerType.TraceLog) => CreateTraceLog(enableFileLogging, filePath, fileName),
                    _ => CreateTraceLog(enableFileLogging, filePath, fileName) // Default to TraceLog if no matching logger type is found.
                };
            }
            catch (Exception ex)
            {
                // Catch any exceptions and rethrow them as a new exception with the original exception as inner exception.
                throw new Exception("Error creating logger.", ex);
            }
        }

        /// <summary>
        /// Creates a Serilog logger instance.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        /// <returns>An instance of the Serilog logger.</returns>
        private static ILoggers CreateSerilog(bool enableFileLogging, string filePath, string fileName) => new MySerilogs(enableFileLogging, filePath, fileName);

        /// <summary>
        /// Creates an NLog logger instance.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        /// <returns>An instance of the NLog logger.</returns>
        private static ILoggers CreateNLog(bool enableFileLogging, string filePath, string fileName) => new MyNLog(enableFileLogging, filePath, fileName);

        /// <summary>
        /// Creates a TraceLog logger instance.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        /// <returns>An instance of the TraceLog logger.</returns>
        private static ILoggers CreateTraceLog(bool enableFileLogging, string filePath, string fileName) => new MyTraceLogger(enableFileLogging, filePath, fileName);
    }
}
