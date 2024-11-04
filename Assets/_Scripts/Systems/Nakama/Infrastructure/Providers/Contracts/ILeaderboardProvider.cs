using Nakama;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILeaderboardProvider
{
    Task<List<IApiLeaderboardRecord>> ListLeaderboardRecordsAsync(string leaderboardId, int limit);
    Task<bool> WriteLeaderboardRecordAsync(string leaderboardId, int score);
}
