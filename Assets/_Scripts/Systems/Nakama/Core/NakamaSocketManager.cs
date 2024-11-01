using Nakama;
using System.Threading.Tasks;
using UnityEngine;

public class NakamaSocketManager
{
    private ISocket socket;
    private NakamaSession sessionManager;

    public NakamaSocketManager(NakamaSession session)
    {
        sessionManager = session;
        socket = NakamaClient.Client.NewSocket();
    }

    public async Task ConnectSocketAsync()
    {
        await socket.ConnectAsync(sessionManager.Session);
        Debug.Log("Socket connected");
    }

    public async Task DisconnectSocketAsync()
    {
        await socket.CloseAsync();
        Debug.Log("Socket disconnected");
    }

    public ISocket GetSocket() => socket;
}
