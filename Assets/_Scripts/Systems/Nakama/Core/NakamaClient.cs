using Nakama;
using UnityEngine;

public class NakamaClient
{
    private static IClient client;
    private static NakamaConfig nakamaConfig;

    public static void Initialize(NakamaConfig config)
    {
        nakamaConfig = config;
        client = new Client(nakamaConfig.scheme, nakamaConfig.host, nakamaConfig.port, nakamaConfig.serverKey);
    }

    public static IClient Client
    {
        get
        {
            if (client == null)
            {
                Debug.LogError("NakamaClient has not been initialized. Call Initialize first.");
                return null;
            }
            return client;
        }
    }
}
