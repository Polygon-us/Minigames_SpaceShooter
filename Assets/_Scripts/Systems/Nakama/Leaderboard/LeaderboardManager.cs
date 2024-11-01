using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Nakama;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Space]
    [SerializeField] private string leaderboardId;

    [Header("UI")]
    [SerializeField] private GameObject panelEnd;
    [SerializeField] private GameObject panelSubmit;
    [SerializeField] private GameObject panelSubmiting;
    [SerializeField] private GameObject panelSubmited;

    [Space]
    [SerializeField] private TMP_InputField nameInput;

    [Space]
    [SerializeField] private GameObject recordPrefab;
    [SerializeField] private Transform recordContainer;

    private bool isOn = false;
    private LeaderboardService leaderboardService;

    private void Awake()
    {
        panelEnd.SetActive(true);
        panelSubmit.SetActive(false);
        panelSubmiting.SetActive(false);
        panelSubmited.SetActive(false);
    }

    public void SetLeaderboardStatus(bool on)
    {
        Leaderboard(on).Forget();
    }

    public void ExitLeaderboard(bool on)
    {
        isOn = on;
        UiManager.Instance.SetUi(UiType.Leaderboard, on, 0.5f);
    }

    public async UniTask Leaderboard(bool on)
    {
        isOn = on;

        if (on)
        {
            await SetLeaderboardRecords();
        }

        UiManager.Instance.SetUi(UiType.Leaderboard, on, 0.5f);
    }

    private async UniTask SetLeaderboardRecords()
    {

        for (int i = recordContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(recordContainer.GetChild(i).gameObject);
        }

        try
        {
            List<IApiLeaderboardRecord> leaderboardRecords = await LeaderboardService.Instance.GetLeaderboardAsync(leaderboardId);

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

    private string FormatNumber(string input)
    {
        if (int.TryParse(input, out int number))
        {
            return number.ToString("N0");
        }
        return input;
    }

    public void StartUploadScore()
    {
        if (PlayerController.Instance.health > PlayerController.Instance._properties.health)
        {
            return;
        }

        panelEnd.SetActive(false);
        panelSubmit.SetActive(false);
        panelSubmiting.SetActive(true);
        panelSubmited.SetActive(false);

        UploadLeaderboard().Forget();
    }

    private async UniTask UploadLeaderboard()
    {
        try
        {
            IApiLeaderboardRecord leaderboardRecord = await LeaderboardService.Instance.WriteLeaderboardRecordAsync(leaderboardId, GameManager.Instance.score);

            if (leaderboardRecord != null)
            {
                panelEnd.SetActive(false);
                panelSubmit.SetActive(false);
                panelSubmiting.SetActive(false);
                panelSubmited.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error subiendo el score al leaderboard: " + ex.Message);
        }
    }
}