using Networking.Nakama.Infrastructure.NakamaAdapters;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WriteLeaderboardUseCase
{
    private readonly ILeaderboardProvider leaderboardProvider;
    private readonly ILogProvider logProvider;

    public WriteLeaderboardUseCase(ILeaderboardProvider leaderboardProvider, ILogProvider logProvider)
    {
        this.leaderboardProvider = leaderboardProvider;
        this.logProvider = logProvider;
    }

    /// <summary>
    /// Writes a leaderboard record with the specified score.
    /// </summary>
    /// <param name="leaderboardId">The ID of the leaderboard.</param>
    /// <param name="score">The score to be recorded.</param>
    /// <returns>A boolean indicating whether the record was successfully written.</returns>
    public async Task<bool> WriteLeaderboardRecordsAsync(string leaderboardId, int score)
    {
        logProvider.LogUseCaseStart(nameof(WriteLeaderboardRecordsAsync), $"Attempting to write a record to leaderboard with ID: {leaderboardId} and score: {score}");
        bool response = false;

        try
        {
            response = await leaderboardProvider.WriteLeaderboardRecordAsync(leaderboardId, score);
            return response;
        }
        catch (System.Exception ex)
        {
            logProvider.LogError(ex, "An error occurred while writing the leaderboard record.");
            return false;
        }
        finally
        {
            string recordsInfo = response ? $"Successfully recorded in leaderboard with ID: {leaderboardId} and score: {score}." : "No record was saved.";
            logProvider.LogUseCaseEnd(nameof(WriteLeaderboardRecordsAsync), recordsInfo);
        }
    }
}
