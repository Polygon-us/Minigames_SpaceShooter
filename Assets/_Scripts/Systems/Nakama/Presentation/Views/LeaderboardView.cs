using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using Nakama;
using TMPro;

public class LeaderboardView : MonoBehaviour
{
    [Space]
    [SerializeField] private string leaderboardId;

    [Space]
    [SerializeField] private GameObject recordPrefab;
    [SerializeField] private Transform recordContainer;

    private bool isOn = false;

    public void SetLeaderboardStatus(bool on)
    {
        ListLeaderboardRecords(on).Forget();
    }

    public void ExitLeaderboard(bool on)
    {
        isOn = on;
        UiManager.Instance.SetUi(UiType.Leaderboard, on, 0.5f);
    }

    private async UniTask ListLeaderboardRecords(bool on)
    {
        isOn = on;

        if (on)
        {
            for (int i = recordContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(recordContainer.GetChild(i).gameObject);
            }

            try
            {
                List<IApiLeaderboardRecord> leaderboardRecords = await new LeaderboardContainer(NetworkManager.Instance)
                    .ListLeaderboardRecordsUseCase()
                    .ListLeaderboardRecordsAsync(leaderboardId);

                for (int i = 0; i < leaderboardRecords.Count; i++)
                {
                    var record = leaderboardRecords[i];
                    string playerRank = record.Rank.ToString();
                    string playerScore = FormatNumber(record.Score.ToString());
                    string playerName = record.Username;

                    var recordInstance = Instantiate(recordPrefab, recordContainer);
                    recordInstance.GetComponent<RecordChild>().SetRecord(playerRank, playerScore, playerName);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error obteniendo registros del leaderboard: " + ex.Message);
            }
        }

        UiManager.Instance.SetUi(UiType.Leaderboard, on, 0.5f);
    }

    public void StartUploadScore()
    {
        if (PlayerController.Instance.health > PlayerController.Instance._properties.health)
        {
            return;
        }

        UploadLeaderboard().Forget();
    }

    private async UniTask UploadLeaderboard()
    {
        try
        {
            await new LeaderboardContainer(NetworkManager.Instance).WriteLeaderboardRecordsUseCase()
                .WriteLeaderboardRecordsAsync(leaderboardId, GameManager.Instance.score);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error subiendo el score al leaderboard: " + ex.Message);
        }
    }
    private string FormatNumber(string input)
    {
        if (int.TryParse(input, out int number))
        {
            return number.ToString("N0");
        }
        return input;
    }
}
