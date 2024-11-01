using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using UnityEngine.SocialPlatforms.Impl;
using Nakama;
public class NakamaConnection : MonoBehaviour
{
    public static NakamaConnection Instance { get; private set; }

    public NakamaConfig nakamaConfig;
    private NakamaSessionManager sessionManager;
    private NakamaSocketManager socketManager;
    private AuthenticationService authService;
    private LeaderboardService leaderboardService;

    public event Action OnNakamaInitialized;
    public LeaderboardService GetLeaderboardService() => leaderboardService;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (nakamaConfig == null)
        {
            Debug.LogError("NakamaConfig is not set.");
            return;
        }

        NakamaClient.Initialize(nakamaConfig);
    }

    private async void Start()
    {
        sessionManager = new NakamaSessionManager();
        socketManager = new NakamaSocketManager(sessionManager);
        authService = new AuthenticationService(sessionManager);
        //matchmakingService = new MatchmakingService(socketManager);
        try
        {
            await authService.Authenticate();
            await socketManager.ConnectSocketAsync();
            leaderboardService = new LeaderboardService(NakamaClient.Client, sessionManager.Session);
            OnNakamaInitialized?.Invoke();

            //var match = await matchmakingService.CreateMatchAsync();
            //matchmakingService.ListenToMatchEvents(match);

            //var writeLeaderboardRecordAsync = await leaderboardService.WriteLeaderboardRecordAsync("4ec4f126-3f9d-11e7-84ef-b7c182b36521", 100);

            //var getLeaderboardAsync = await leaderboardService.GetLeaderboardAsync("4ec4f126-3f9d-11e7-84ef-b7c182b36521");
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError("NullReferenceException: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
        DisconnectSocketAsync().Forget();
    }

    private async Task DisconnectSocketAsync()
    {
        try
        {
            await socketManager.DisconnectSocketAsync();
            Debug.Log("Disconnected from Nakama socket.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error disconnecting from Nakama socket: " + ex.Message);
        }
    }

}

public static class TaskExtensions
{
    public static void Forget(this Task task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Task faulted: " + task.Exception?.GetBaseException().Message);
        }
    }
}