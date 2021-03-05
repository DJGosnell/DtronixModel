using System;
using System.Diagnostics;

namespace DtronixModel
{
    /// <summary>
    /// Simple logging interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="entry">Data about the logged event.</param>
        void Log(LogEntry entry);
    }

    /// <summary>
    /// Log event data
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Level of logged event
        /// </summary>
        public readonly LogEntryEventType Severity;

        /// <summary>
        /// Logged event message
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// Exception if one has occurred.  Null otherwise.
        /// </summary>
        public readonly Exception Exception;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogEntry()
        {
            
        }

        /// <summary>
        /// Creates a new log entry with the specified contents.
        /// </summary>
        /// <param name="severity">Level of logged event</param>
        /// <param name="message">Logged event message</param>
        /// <param name="exception">Exception if one has occurred.  Null otherwise.</param>
        public LogEntry(LogEntryEventType severity, string message, Exception exception = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message == string.Empty)
                throw new ArgumentException("empty", nameof(message));

            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }

    /// <summary>
    /// Contains all the levels a logged event can be set to.
    /// </summary>
    public enum LogEntryEventType
    {
        /// <summary>
        /// Trace level logging.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Debug level logging.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Info level logging.
        /// </summary>
        Info = 2,

        /// <summary>
        /// Warning level logging.
        /// </summary>
        Warn = 3,

        /// <summary>
        /// Error level logging.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Fatal level logging.
        /// </summary>
        Fatal = 5
    }

    internal static class LoggerExtensions
    {
        // Trace
        public static void Trace(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Trace, exception.Message, exception));
        }

        public static void Trace(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Trace, message));
        }

        public static void Trace(this ILogger logger, Exception exception, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Trace, message, exception));
        }

        // Debug
        public static void Debug(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Debug, exception.Message, exception));
        }

        public static void Debug(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Debug, message));
        }

        public static void Debug(this ILogger logger, Exception exception, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Debug, message, exception));
        }

        // Info
        public static void Info(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Info, exception.Message, exception));
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Info, message));
        }

        public static void Info(this ILogger logger, Exception exception, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Info, message, exception));
        }

        // Warn
        public static void Warn(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Warn, exception.Message, exception));
        }

        public static void Warn(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Warn, message));
        }

        public static void Warn(this ILogger logger, Exception exception, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Warn, message, exception));
        }

        // Error
        public static void Error(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Error, exception.Message, exception));
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Error, message));
        }

        public static void Error(this ILogger logger, Exception exception, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Error, message, exception));
        }

        // Fatal
        public static void Fatal(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Fatal, exception.Message, exception));
        }

        public static void Fatal(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Fatal, message));
        }

        public static void Fatal(this ILogger logger, Exception exception, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Fatal, message, exception));
        }

        [Conditional("DEBUG")]
        public static void ConditionalDebug(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Debug, exception.Message, exception));
        }

        [Conditional("DEBUG")]
        public static void ConditionalDebug(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Debug, message));
        }

        [Conditional("DEBUG")]
        public static void ConditionalDebug(this ILogger logger, string message, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Debug, message, exception));
        }

        [Conditional("DEBUG")]
        public static void ConditionalTrace(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LogEntryEventType.Trace, message));
        }

        [Conditional("DEBUG")]
        public static void ConditionalTrace(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Trace, exception.Message, exception));
        }

        [Conditional("DEBUG")]
        public static void ConditionalTrace(this ILogger logger, string message, Exception exception)
        {
            logger.Log(new LogEntry(LogEntryEventType.Trace, message, exception));
        }
    }
}