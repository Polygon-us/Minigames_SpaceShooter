using Nakama;
using UnityEngine;

namespace Networking.Nakama.Infrastructure.NakamaAdapters
{
    /// <summary>
    /// LogProvider is responsible for logging messages and errors related to Nakama interactions.
    /// </summary>
    public class LogProvider : ILogProvider
    {
        // Constructor público para permitir la inyección de dependencias
        public LogProvider() { }

        // Log levels
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The message to log.</param>
        public void Log(LogLevel level, string message)
        {
            string formattedMessage = $"{System.DateTime.Now}: [{level}] {message}";

            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log($"<color=#ADD8E6>ℹ️ {formattedMessage}</color>");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"<color=orange>⚠️ {formattedMessage}</color>");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"<color=red>❌ {formattedMessage}</color>");
                    break;
                default:
                    Debug.Log(formattedMessage);
                    break;
            }
        }

        public void LogError(System.Exception exception, string message = "")
        {
            string errorMessage = string.IsNullOrEmpty(message)
                ? exception.Message
                : $"{message}: {exception.Message}";

            Debug.LogError($"<color=red>{System.DateTime.Now}: [Error] {errorMessage}\n{exception.StackTrace}</color>");
        }

        public void LogInfo(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void LogWarning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public void LogLeaderboardRecord(IApiLeaderboardRecord record)
        {
            string details = $"Rank: {record.Rank}, User ID: {record.OwnerId}, Username: {record.Username}, " +
                             $"Score: {record.NumScore}, Subscore: {record.Subscore}, " +
                             $"Created: {record.CreateTime}, Updated: {record.UpdateTime}, " +
                             $"Expires: {record.ExpiryTime}, Metadata: {record.Metadata}";

            LogInfo($"Leaderboard Record: {details}");
        }

        // Métodos para registrar el inicio y fin de un caso de uso
        public void LogUseCaseStart(string useCaseName, string parameters)
        {
            Log(LogLevel.Info, $"🚀 Starting use case: {useCaseName} with parameters: {parameters}");
        }

        public void LogUseCaseEnd(string useCaseName, string parameters)
        {
            Log(LogLevel.Info, $"✅ Ending use case: {useCaseName} with parameters: {parameters}");
        }
    }
}
