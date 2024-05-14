using Serilog.Events;
using SwitchLoggers.Enums;
using SwitchLoggers.Loggers.Interfaces;
using SwitchLoggers.Loggers.TraceLogger;

namespace SwitchLoggers.Settings
{
    /// <summary>
    /// Configuration class for managing the logger instance throughout the application.
    /// </summary>
    public class SwitchLoggersSettings
    {

        private static readonly List<string> _listOfLoggers = PopulateListOfLoggers();

        /// <summary>
        /// Gets or sets the current logger instance.
        /// </summary>
        public static ILoggers Logger { get; set; } = new MyTraceLogger(false, string.Empty, string.Empty);

        /// <summary>
        /// Populate List with all permitted loggers.
        /// </summary>
        public static IReadOnlyList<string> ListOfLoggers => _listOfLoggers;

        /// <summary>
        /// Gets the current date and time in the format "yyyy-MM-dd HH:mm:ss".
        /// </summary>
        public static string DateTimeNow { get => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }

        /// <summary>
        /// Gets the console logging level for NLog.
        /// </summary>
        public static NLog.LogLevel ConsoleNLogLevel { get; set; } = NLog.LogLevel.Trace;

        /// <summary>
        /// Gets the file logging level for NLog.
        /// </summary>
        public static NLog.LogLevel FileNLogLevel { get; set; } = NLog.LogLevel.Trace;

        /// <summary>
        /// Gets the console logging level for Serilog.
        /// </summary>
        public static LogEventLevel ConsoleSerilogLevel { get; set; } = LogEventLevel.Verbose;

        /// <summary>
        /// Gets the file logging level for Serilog.
        /// </summary>
        public static LogEventLevel FileSerilogLevel { get; set; } = LogEventLevel.Verbose;

        /// <summary>
        /// Gets or sets a value indicating whether file logging is enabled.
        /// </summary>
        public static bool EnableFileLogging { get; set; } = false;

        /// <summary>
        /// Gets or sets the file path for logging.
        /// </summary>
        public static string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file name for logging.
        /// </summary>
        public static string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Populates a list of string representations of logger types.
        /// </summary>
        /// <returns>A list of string representations of logger types.</returns>
        private static List<string> PopulateListOfLoggers()
        {
            // Clear list to initialize.
            List<string> list = new();

            // Iterate through each logger type in the enum and add its string representation to the list.
            foreach (LoggerType logger in Enum.GetValues(typeof(LoggerType)))
                list.Add(logger.ToString());

            // Return the populated list.
            return list;
        }
    }
}