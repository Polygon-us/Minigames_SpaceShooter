using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Nakama;

public class LeaderboardService
{
    private readonly IClient client;
    private readonly ISession session; 

    public LeaderboardService(IClient client, ISession session)
    {
        this.client = client;
        this.session = session;
    }

    public async Task<IApiLeaderboardRecord> WriteLeaderboardRecordAsync(string leaderboardId, long score, long subScore = 0L, string metadata = null)
    {
        if (session == null || session.IsExpired)
        {
            Debug.LogError("La sesión es nula o ha expirado.");
            return null;
        }

        var record = await client.WriteLeaderboardRecordAsync(session, leaderboardId, score, subScore, metadata);
        return record;
    }

    public async Task<List<IApiLeaderboardRecord>> GetLeaderboardAsync(string leaderboardId, int limit = 10)
    {
        if (session == null || session.IsExpired)
        {
            Debug.LogError("La sesión es nula o ha expirado.");
            return null;
        }

        Debug.Log("session: "+ session);
        Debug.Log("leaderboardId: " + leaderboardId);

        IApiLeaderboardRecordList apiLeaderboardRecordList = await client.ListLeaderboardRecordsAsync(session, leaderboardId, limit: limit);
        return apiLeaderboardRecordList.Records.ToList();
    }


   
}
