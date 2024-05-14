using SwitchLoggers.Loggers.NLogger;
using SwitchLoggers.Loggers.Serilogs;
using SwitchLoggers.Loggers.TraceLogger;

namespace SwitchLoggers.Enums
{
    /// <summary>
    /// Enum representing different types of loggers.
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// NLog logger.
        /// Implemented in <see cref="MyNLog"/> class
        /// </summary>
        NLog,

        /// <summary>
        /// Serilog logger.
        /// Implemented in <see cref="MySerilogs"/> class
        /// </summary>
        Serilog,

        /// <summary>
        /// TraceLog logger.
        /// Implemented in <see cref="MyTraceLogger"/> class
        /// </summary>
        TraceLog
    }
}