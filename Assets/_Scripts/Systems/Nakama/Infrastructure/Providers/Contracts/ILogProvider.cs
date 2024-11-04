using System;
using Nakama;

namespace Networking.Nakama.Infrastructure.NakamaAdapters
{
    public interface ILogProvider
    {
        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The message to log.</param>
        void Log(LogProvider.LogLevel level, string message);

        /// <summary>
        /// Logs an exception with a custom message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional custom message to log with the exception.</param>
        void LogError(Exception exception, string message = "");

        /// <summary>
        /// Logs informational messages related to Nakama operations.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs warnings related to potential issues in Nakama operations.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs the start of a use case.
        /// </summary>
        /// <param name="useCaseName">The name of the use case that started.</param>
        void LogUseCaseStart(string useCaseName, string parameters);

        /// <summary>
        /// Logs the end of a use case.
        /// </summary>
        /// <param name="useCaseName">The name of the use case that completed.</param>
        void LogUseCaseEnd(string useCaseName, string response);
    }
}
