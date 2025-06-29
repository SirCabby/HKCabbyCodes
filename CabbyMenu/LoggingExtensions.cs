using BepInEx.Logging;
using System;

namespace CabbyMenu
{
    /// <summary>
    /// Extension methods for structured logging with different levels.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Logs a debug message with structured formatting.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogDebug(this ManualLogSource logger, string message, params object[] args)
        {
            if (logger != null)
            {
                logger.Log(LogLevel.Debug, string.Format(message, args));
            }
        }

        /// <summary>
        /// Logs an info message with structured formatting.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogInfo(this ManualLogSource logger, string message, params object[] args)
        {
            if (logger != null)
            {
                logger.Log(LogLevel.Info, string.Format(message, args));
            }
        }

        /// <summary>
        /// Logs a warning message with structured formatting.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogWarning(this ManualLogSource logger, string message, params object[] args)
        {
            if (logger != null)
            {
                logger.Log(LogLevel.Warning, string.Format(message, args));
            }
        }

        /// <summary>
        /// Logs an error message with structured formatting.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogError(this ManualLogSource logger, string message, params object[] args)
        {
            if (logger != null)
            {
                logger.Log(LogLevel.Error, string.Format(message, args));
            }
        }

        /// <summary>
        /// Logs an exception with context information.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="ex">The exception to log.</param>
        /// <param name="context">Context information about where the exception occurred.</param>
        public static void LogException(this ManualLogSource logger, Exception ex, string context = "")
        {
            if (logger != null && ex != null)
            {
                string contextInfo = string.IsNullOrEmpty(context) ? "" : string.Format("Context: {0}. ", context);
                logger.Log(LogLevel.Error, string.Format("{0}Exception: {1}", contextInfo, ex.Message));
                logger.Log(LogLevel.Error, string.Format("Stack Trace: {0}", ex.StackTrace));
            }
        }

        /// <summary>
        /// Logs performance timing information.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="operation">Name of the operation being timed.</param>
        /// <param name="elapsedMs">Elapsed time in milliseconds.</param>
        public static void LogPerformance(this ManualLogSource logger, string operation, double elapsedMs)
        {
            if (logger != null)
            {
                logger.Log(LogLevel.Debug, string.Format("Performance: {0} took {1:F2}ms", operation, elapsedMs));
            }
        }

        /// <summary>
        /// Logs method entry for debugging.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="methodName">Name of the method being entered.</param>
        /// <param name="parameters">Method parameters.</param>
        public static void LogMethodEntry(this ManualLogSource logger, string methodName, params object[] parameters)
        {
            if (logger != null)
            {
                string paramInfo = parameters.Length > 0 ? string.Format(" with params: [{0}]", string.Join(", ", parameters)) : "";
                logger.Log(LogLevel.Debug, string.Format("Entering method: {0}{1}", methodName, paramInfo));
            }
        }

        /// <summary>
        /// Logs method exit for debugging.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="methodName">Name of the method being exited.</param>
        /// <param name="result">Method result (optional).</param>
        public static void LogMethodExit(this ManualLogSource logger, string methodName, object result = null)
        {
            if (logger != null)
            {
                string resultInfo = result != null ? string.Format(" with result: {0}", result) : "";
                logger.Log(LogLevel.Debug, string.Format("Exiting method: {0}{1}", methodName, resultInfo));
            }
        }
    }
}