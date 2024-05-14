using SwitchLoggers.Enums;
using SwitchLoggers.Loggers.Interfaces;
using SwitchLoggers.Settings;
using SwitchLoggers.Utilities;
using System.Diagnostics;

namespace SwitchLoggers.Loggers.TraceLogger
{
    /// <summary>
    /// Implementation of the <see cref="ILoggers"/> and <see cref="IFactory_Loggers"/> interfaces using Serilog.
    /// </summary>
    public class MyTraceLogger : ILoggers, IFactory_Loggers
    {
        /// <summary>
        /// Constructor for MyTraceLogger.
        /// </summary>
        /// <param name="enableFileLogging">A boolean indicating whether file logging is enabled.</param>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        public MyTraceLogger(bool enableFileLogging, string filePath, string fileName)
        {
            // Build the Trace Logger during initialization.
            BuildTraceLogger(enableFileLogging, filePath, fileName);

            // Check if logger has been enabled to save data in a file.
            if (!enableFileLogging)
                Trace.WriteLine($"[WARNING {DateTime.Now:yyyy-MM-dd HH:mm:ss} ]: Any file has been defined to save logs yet.");
        }

        /// <inheritdoc/>
        public ILoggers CreateLogger(string logName, bool enableFileLogging, string filePath, string fileName)
        {
            // This implementation simply returns the current instance of the LoggerFactory,
            // assuming that it has already been configured elsewhere to create the appropriate logger.
            return this;
        }

        /// <inheritdoc/>
        public void LogDebug(string message)
        {
            Trace.WriteLine($"[DEBUG   {SwitchLoggersSettings.DateTimeNow} ]: {message}");
        }

        /// <inheritdoc/>
        public void LogError(string message)
        {
            Trace.WriteLine($"[ERROR   {SwitchLoggersSettings.DateTimeNow} ]: {message}");
        }

        /// <inheritdoc/>
        public void LogFatal(string message)
        {
            Trace.WriteLine($"[FATAL   {SwitchLoggersSettings.DateTimeNow} ]: {message}");
        }

        /// <inheritdoc/>
        public void LogInformation(string message)
        {
            Trace.WriteLine($"[INFO    {SwitchLoggersSettings.DateTimeNow} ]: {message}");
        }

        /// <inheritdoc/>
        public void LogVerbose(string message)
        {
            Trace.WriteLine($"[VERBOSE {SwitchLoggersSettings.DateTimeNow} ]: {message}");
        }

        /// <inheritdoc/>
        public void LogWarning(string message)
        {
            Trace.WriteLine($"[WARNING {SwitchLoggersSettings.DateTimeNow} ]: {message}");
        }

        /// <summary>
        /// Builds a trace logger.
        /// </summary>
        /// <param name="filePath">The path where log files will be stored.</param>
        /// <param name="fileName">The name of the log file.</param>
        private static void BuildTraceLogger(bool enableFileLogging, string filePath, string fileName)
        {
            // Don't do anything if it's true.
            if (!enableFileLogging)
            {
                return;
            }

            // Close any existing trace listeners.
            foreach (TraceListener listener in Trace.Listeners)
            {
                listener.Close();
            }

            // Set a default path if it's empty or contains invalid characters.
            var (path, file) = Utilities_Loggers.SetCompletePath(nameof(LoggerType.TraceLog), filePath, fileName);

            // Create the directory if it does not exist.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Combine the file path and file name.
            string completePath = Path.Combine(@path, @file);

            // Create a new TextWriterTraceListener with the combined file path and file name.
            using (TextWriterTraceListener textWriterTraceListener = new(completePath))
            {
                // Add the TextWriterTraceListener to the list of trace listeners.
                Trace.Listeners.Add(textWriterTraceListener);

                // Ensure that trace messages are flushed immediately.
                Trace.AutoFlush = true;
            }
        }
    }
}