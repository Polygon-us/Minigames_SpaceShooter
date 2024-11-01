using Nakama;
using UnityEngine;

public class NakamaClient
{
    private static IClient client;
    private static NakamaConfig nakamaConfig;

    public static void Initialize(NakamaConfig config)
    {
        nakamaConfig = config;
    }

    public static IClient Client
    {
        get
        {
            if (client == null)
            {
                client = new Client(nakamaConfig.scheme, nakamaConfig.host, nakamaConfig.port, nakamaConfig.serverKey);
            }
            return client;
        }
    }
}
