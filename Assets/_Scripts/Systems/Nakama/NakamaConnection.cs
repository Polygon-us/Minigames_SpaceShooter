using System.Threading.Tasks;
using UnityEngine;
using Nakama;

public class NakamaConnection : MonoBehaviour
{
    private static NakamaConnection instance;
    private NakamaSession sessionManager;
    [SerializeField] private NakamaConfig nakamaConfig;

    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (nakamaConfig == null)
            {
                Debug.LogError("NakamaConfig no está asignado en el inspector.");
                return;
            }
            NakamaClient.Initialize(nakamaConfig);
            sessionManager = gameObject.AddComponent<NakamaSession>();
            await sessionManager.AuthenticateDeviceAsync();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static NakamaConnection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<NakamaConnection>();
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("NakamaConnection");
                    instance = gameObject.AddComponent<NakamaConnection>();
                }
            }
            return instance;
        }
    }

    public IClient Client => NakamaClient.Client;
    public ISession Session => sessionManager.Session;

}