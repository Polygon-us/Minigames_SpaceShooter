using Networking.Nakama.Infrastructure.NakamaAdapters;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Nakama;

public class StatusServer : MonoBehaviour
{
    private Toggle connectionToggle;
    [SerializeField] private TextMeshProUGUI statusServerText;
    private Coroutine connectionCheckCoroutine;

    private void Awake()
    {
        connectionToggle = GetComponent<Toggle>();
        NetworkManager.OnConnectionStatusChanged += UpdateToggleState;
    }

    private void OnDestroy()
    {
        NetworkManager.OnConnectionStatusChanged -= UpdateToggleState;
        Debug.Log("StatusServer: Unsubscribed from connection status changes.");

        if (connectionCheckCoroutine != null)
        {
            StopCoroutine(connectionCheckCoroutine);
        }
    }

    private void UpdateToggleState(bool isConnected)
    {
        connectionToggle.isOn = isConnected;
        connectionToggle.gameObject.SetActive(isConnected);

        if (isConnected)
        {
            ISession session = NetworkManager.Instance.GetClient().GetSession().GetInstance();
            statusServerText.text = $"Connected: \n{session.Username}";
        }
    }
}
