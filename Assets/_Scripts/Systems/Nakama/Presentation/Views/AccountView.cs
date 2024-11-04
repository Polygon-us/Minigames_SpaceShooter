using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
using Nakama;
using System.Threading.Tasks;

public class AccountView : MonoBehaviour 
{
    public void SetLeaderboardStatus()
    {
        ExitLeaderboard().Forget();
    }

    public async UniTask ExitLeaderboard()
    {
        IApiAccount account = await new AccountContainer(NetworkManager.Instance)
                    .GetAccountUseCase()
                    .GetAccountAsync();

        Debug.Log("account: " + account);
    }
}
