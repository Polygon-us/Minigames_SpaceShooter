using System;
using UnityEngine;

public static class GameLogger
{
    public static void LogEvent(string system, string message)
    {
        Debug.Log($"[ℹ️ {system}] {message}");
    }

    public static void LogWarning(string system, string message)
    {
        Debug.LogWarning($"[⚠️ {system}] {message}");
    }

    public static void LogError(string system, string message, Exception ex = null)
    {
        var errorMessage = ex == null ? message : $"{message} - {ex.Message}\n{ex.StackTrace}";
        Debug.LogError($"[❗ {system}] {errorMessage}");
    }
}