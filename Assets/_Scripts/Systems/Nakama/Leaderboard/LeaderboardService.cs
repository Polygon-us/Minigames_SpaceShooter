using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Nakama;

public class LeaderboardService : MonoBehaviour
{
    private static LeaderboardService instance;
    private IClient client;
    private ISession session; 

    private void Awake()
    {
        client = NakamaConnection.Instance.Client;
        session = NakamaConnection.Instance.Session;
    }

    public static LeaderboardService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<LeaderboardService>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject("LeaderboardService");
                    instance = gameObject.AddComponent<LeaderboardService>();
                }
            }
            return instance;
        }
    }

    public async Task<IApiLeaderboardRecord> WriteLeaderboardRecordAsync(string leaderboardId, long score, long subScore = 0L, string metadata = null)
    {
        if (session == null || session.IsExpired)
        {
            Debug.LogError("La sesi�n es nula o ha expirado.");
            return null;
        }

        var record = await client.WriteLeaderboardRecordAsync(session, leaderboardId, score, subScore, metadata);
        return record;
    }

    public async Task<List<IApiLeaderboardRecord>> GetLeaderboardAsync(string leaderboardId, int limit = 10)
    {
        if (session == null || session.IsExpired)
        {
            Debug.LogError("The session is null or has expired.");
            return null;
        }

        IApiLeaderboardRecordList apiLeaderboardRecordList = await client.ListLeaderboardRecordsAsync(session, leaderboardId, limit: limit);
        return apiLeaderboardRecordList.Records.ToList();
    }


   
}
