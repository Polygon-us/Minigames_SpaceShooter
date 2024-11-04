using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Nakama;
using Networking.Nakama.Infrastructure.NakamaAdapters;
using UnityEngine.Profiling;

public class ListLeaderboardUseCase
{
    private readonly ILeaderboardProvider leaderboardProvider;
    private readonly ILogProvider logProvider;

    public ListLeaderboardUseCase(ILeaderboardProvider leaderboardProvider, LogProvider logProvider)
    {
        this.leaderboardProvider = leaderboardProvider;
        this.logProvider = logProvider;
    }

    public async Task<List<IApiLeaderboardRecord>> ListLeaderboardRecordsAsync(string leaderboardId)
    {
        logProvider.LogUseCaseStart(nameof(ListLeaderboardRecordsAsync), $"leaderboardId: {leaderboardId}");
        List<IApiLeaderboardRecord> response = new List<IApiLeaderboardRecord>();

        try
        {
            response = await leaderboardProvider.ListLeaderboardRecordsAsync(leaderboardId, 10);
            return response;
        }
        catch (System.Exception ex)
        {
            logProvider.LogError(ex, "Error fetching leaderboard records");
            return new List<IApiLeaderboardRecord>();
        }
        finally
        {
            string recordsInfo = response.Count > 0 ? $"Records Count: {response.Count}" : "No records found";
            logProvider.LogUseCaseEnd(nameof(ListLeaderboardRecordsAsync), recordsInfo);
        }
    }
}
