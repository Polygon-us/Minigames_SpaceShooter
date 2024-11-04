using Nakama;
using Networking.Nakama.Infrastructure.NakamaAdapters;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetAccountUseCase
{
    private readonly IAccountProvider accountProvider;
    private readonly ILogProvider logProvider;

    public GetAccountUseCase(IAccountProvider accountProvider, LogProvider logProvider)
    {
        this.accountProvider = accountProvider;
        this.logProvider = logProvider;
    }

    /// <summary>
    /// Fetches the account information asynchronously.
    /// </summary>
    /// <returns>An instance of IApiAccount containing account details, or null if not found.</returns>
    public async Task<IApiAccount> GetAccountAsync()
    {
        logProvider.LogUseCaseStart(nameof(GetAccountAsync), "Initiating account retrieval."); 
        IApiAccount response = null;

        try
        {
            response = await accountProvider.GetAccountAsync();
            logProvider.LogInfo("Account retrieved successfully.: "+ response);
            return response;
        }
        catch (Exception ex)
        {
            logProvider.LogError(ex, "An error occurred while fetching the account details.");
            return null;
        }
        finally
        {
            if (response != null)
            {
                string accountInfo = $"Account Retrieved: " +
                                     $"Custom ID: {response.CustomId}, " +
                                     $"Email: {response.Email}, " +
                                     $"Disable Time: {response.DisableTime}, " +
                                     $"Verify Time: {response.VerifyTime}, " +
                                     $"Wallet: {response.Wallet}, " +
                                     $"Devices Count: {response.Devices?.Count()}";

                logProvider.LogUseCaseEnd(nameof(GetAccountAsync), accountInfo);
            }
            else
            {
                logProvider.LogUseCaseEnd(nameof(GetAccountAsync), "No account details retrieved.");
            }
        }
    }
}
