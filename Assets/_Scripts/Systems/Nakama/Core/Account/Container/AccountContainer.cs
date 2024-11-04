using Networking.Nakama.Infrastructure.NakamaAdapters;
using UnityEngine;

public class AccountContainer
{
    private readonly LogProvider logProvider;
    private readonly AccountProvider accountProvider;

    public AccountContainer(NetworkManager networkManager)
    {
        Debug.Log("networkManager CLienakldfnklasnd: " + networkManager.GetClient());
        Debug.Log("networkManager aseaosdbajksdb: " + networkManager.GetClient().GetSession());

        logProvider = new LogProvider();
        accountProvider = new AccountProvider(networkManager, logProvider);
    }

    public GetAccountUseCase GetAccountUseCase()
    {
        return new GetAccountUseCase(accountProvider, logProvider);
    }
}
