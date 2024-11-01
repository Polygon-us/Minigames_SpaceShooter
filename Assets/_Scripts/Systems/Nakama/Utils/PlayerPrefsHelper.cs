using UnityEngine;

public static class PlayerPrefsHelper
{
    public static string GetDeviceId()
    {
        var deviceId = PlayerPrefs.GetString("deviceId", SystemInfo.deviceUniqueIdentifier);
        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            deviceId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("deviceId", deviceId);
        }
        return deviceId;
    }
}
