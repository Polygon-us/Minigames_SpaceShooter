using Networking.Nakama.Infrastructure.NakamaAdapters;
using System.Threading.Tasks;
using Nakama;
using System;

public class AccountProvider : IAccountProvider
{
    private readonly INakamaClient client;
    private readonly INakamaSession session;
    private readonly ILogProvider logger;

    public AccountProvider(NetworkManager networkManager, ILogProvider logProvider)
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
    public async Task<IApiAccount> GetAccountAsync()
    {
        try
        {
            logger.LogInfo("client: " + client);
            if (client.IsConnected())
            {
                logger.LogInfo("client.IsConnected(): " + client.IsConnected());

                return await client.GetInstance().GetAccountAsync(session.GetInstance());

            }
            logger.LogInfo("client.IsConnected(): " + "null");

            return null;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error writing leaderboard record:");
            return null;
        }
    }
}
