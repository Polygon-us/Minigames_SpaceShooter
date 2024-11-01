using Nakama;
using System.Threading.Tasks;
using UnityEngine;

public class MatchmakingService
{
    private NakamaSocketManager socketManager;

    public MatchmakingService(NakamaSocketManager socket)
    {
        socketManager = socket;
    }

    public async Task<IMatch> CreateMatchAsync()
    {
        var match = await socketManager.GetSocket().CreateMatchAsync();
        Debug.Log($"Joined match with ID: {match.Id}");
        return match;
    }

    public void ListenToMatchEvents(IMatch match)
    {
        socketManager.GetSocket().ReceivedMatchPresence += presenceEvent =>
        {
            foreach (var user in presenceEvent.Joins)
            {
                Debug.Log($"User joined: {user.Username}");
            }
            foreach (var user in presenceEvent.Leaves)
            {
                Debug.Log($"User left: {user.Username}");
            }
        };
    }
}
