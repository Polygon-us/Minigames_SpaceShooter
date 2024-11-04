using Networking.Nakama.Infrastructure.NakamaAdapters;
using System.Threading.Tasks;
using UnityEngine;
using Nakama;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public static event Action<bool> OnConnectionStatusChanged;

    [SerializeField] private NakamaConfig nakamaConfig;
    private INakamaClient client;
    private ILogProvider logger;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        logger = new LogProvider();
        client = new NakamaClient(nakamaConfig, logger);
        Connect();
    }

    /// <summary>
    /// Connects to the Nakama server.
    /// </summary>
    public async void Connect()
    {
        logger.LogInfo("Init connect server");
        try
        {
            var devideId = DeviceIdUtil.GetDeviceId();
            await client.AuthenticateDeviceAsync(devideId, UsernameUtil.GenerateUniqueUsername(devideId));
            OnConnectionStatusChanged?.Invoke(client.IsConnected());
            logger.LogInfo(client.ToString());
        }
        catch (Exception ex)
        {
            OnConnectionStatusChanged?.Invoke(client.IsConnected());
            logger.LogError(ex, "Error connecting to Nakama server");
        }
    }

    /// <summary>
    /// Disconnects from the Nakama server.
    /// </summary>
    public void Disconnect()
    {
        client.Disconnect();
        OnConnectionStatusChanged?.Invoke(client.IsConnected());
        logger.LogWarning("Disconnected from Nakama server.");
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public INakamaClient GetClient()
    {
        return client;
    }
}
