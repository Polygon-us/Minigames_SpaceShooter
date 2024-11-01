using Nakama;
using UnityEngine;

public class NakamaSessionManager
{
    public ISession Session { get; private set; }

    public async System.Threading.Tasks.Task AuthenticateDeviceAsync()
    {
        var deviceId = PlayerPrefs.GetString("deviceId", SystemInfo.deviceUniqueIdentifier);
        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            deviceId = System.Guid.NewGuid().ToString();
        }
        PlayerPrefs.SetString("deviceId", deviceId);

        Session = await NakamaClient.Client.AuthenticateDeviceAsync(deviceId);
        Debug.Log("Authenticated with Device ID: " + deviceId);
    }
}
