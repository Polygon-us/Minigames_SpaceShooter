using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using System;
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

    private const string scorePoint = ".";
    private bool isOn = false;
    private LeaderboardService leaderboardService;
    private NakamaSessionManager sessionManager;

    private async void Awake()
    {
        panelEnd.SetActive(true);
        panelSubmit.SetActive(false);
        panelSubmiting.SetActive(false);
        panelSubmited.SetActive(false);

        while (NakamaConnection.Instance == null)
        {
            Debug.LogWarning("Waiting for NakamaConnection to be initialized...");
            await Task.Delay(100);
        }

        NakamaConnection.Instance.OnNakamaInitialized += InitializeLeaderboardService;
    }

    private void OnDestroy()
    {
        if (NakamaConnection.Instance != null)
        {
            NakamaConnection.Instance.OnNakamaInitialized -= InitializeLeaderboardService;
        }
    }

    private void InitializeLeaderboardService()
    {
        leaderboardService = NakamaConnection.Instance.GetLeaderboardService();

        if (leaderboardService == null)
        {
            Debug.LogError("LeaderboardService no está inicializado.");
        }
    }

    public void SetLeaderboardStatus(bool on)
    {
        if (on && isOn) return;

        Leaderboard(on).Forget();
    }

    private async UniTask Leaderboard(bool on)
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
        Debug.Log("leaderboardService: " + leaderboardService);
        try
        {
            var leaderboardData = await leaderboardService.GetLeaderboardAsync(leaderboardId);
            Debug.Log("leaderboardData: " + leaderboardData);
            for (int i = 0; i < leaderboardData.Count; i++)
            {
                var record = leaderboardData[i];
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
        if (string.IsNullOrEmpty(nameInput.text) || PlayerController.Instance.health > PlayerController.Instance._properties.health)
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
            var leaderboardData = await leaderboardService.WriteLeaderboardRecordAsync(leaderboardId, GameManager.Instance.score);

            if (leaderboardData != null)
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