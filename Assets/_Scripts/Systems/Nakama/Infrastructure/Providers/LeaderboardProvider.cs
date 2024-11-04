using Networking.Nakama.Infrastructure.NakamaAdapters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Nakama;
using System.Diagnostics;
using System;

public class LeaderboardProvider : ILeaderboardProvider
{
    private readonly INakamaClient client;
    private readonly INakamaSession session;
    private readonly ILogProvider logger;

    public LeaderboardProvider(NetworkManager networkManager, ILogProvider logProvider)
    {
        client = networkManager.GetClient();
        session = client?.GetSession();
        logger = logProvider;
    }

    /// <summary>
    /// Gets the leaderboard records from Nakama.
    /// </summary>
    /// <param name="leaderboardId">The ID of the leaderboard.</param>
    /// <param name="limit">The number of records to retrieve.</param>
    /// <returns>A list of leaderboard records.</returns>
    public async Task<List<IApiLeaderboardRecord>> ListLeaderboardRecordsAsync(string leaderboardId, int limit)
    {
        try
        {
            IApiLeaderboardRecordList apiLeaderboardRecordList = await client.GetInstance().ListLeaderboardRecordsAsync(session.GetInstance(), leaderboardId, limit: limit);
            return apiLeaderboardRecordList.Records.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error writing leaderboard record:");
            return new List<IApiLeaderboardRecord>();
        }
    }

    /// <summary>
    /// Writes a new leaderboard record to Nakama.
    /// </summary>
    /// <param name="leaderboardId">The ID of the leaderboard.</param>
    /// <param name="score">The score to submit.</param>
    /// <returns>True if the record was created successfully; otherwise, false.</returns>
    public async Task<bool> WriteLeaderboardRecordAsync(string leaderboardId, int score)
    {
        try
        {
            var hola = await client.GetInstance().WriteLeaderboardRecordAsync(session.GetInstance(), leaderboardId, score);
            logger.LogInfo("hola: " + hola);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error writing leaderboard record:");
            return false;
        }
    }

}
