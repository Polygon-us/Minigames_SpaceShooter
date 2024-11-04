using Networking.Nakama.Infrastructure.NakamaAdapters;

public class LeaderboardContainer
{
    private readonly LogProvider logProvider;
    private readonly LeaderboardProvider leaderboardProvider;

    public LeaderboardContainer(NetworkManager networkManager)
    {
        logProvider = new LogProvider();
        leaderboardProvider = new LeaderboardProvider(networkManager, logProvider);
    }

    public ListLeaderboardUseCase ListLeaderboardRecordsUseCase()
    {
        return new ListLeaderboardUseCase(leaderboardProvider, logProvider);
    }

    public WriteLeaderboardUseCase WriteLeaderboardRecordsUseCase()
    {
        return new WriteLeaderboardUseCase(leaderboardProvider, logProvider);
    }
}
