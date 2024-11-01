using System.Threading.Tasks;
using UnityEngine;
using Nakama;
using System;

public class NakamaSession : MonoBehaviour
{
    public ISession Session { get; private set; }

    public async Task AuthenticateDeviceAsync()
    {
        var deviceId = PlayerPrefs.GetString("deviceId", SystemInfo.deviceUniqueIdentifier);
        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            deviceId = System.Guid.NewGuid().ToString();
        }
        PlayerPrefs.SetString("deviceId", deviceId);

        try
        {
            Session = await NakamaClient.Client.AuthenticateDeviceAsync(deviceId, username: GenerateUniqueUsername());
        }
        catch (Exception e)
        {
            Debug.LogError("Error during authentication: " + e.Message);
        }
    }

    private string GenerateUniqueUsername()
    {
        string[] prefixes = { "Astro", "Nova", "Star", "Galaxy", "Nebula", "Comet", "Orion", "Quantum", "Photon", "Pulsar", "Cosmic", "Solar", "Rocket", "Void" };
        string[] suffixes = { "Commander", "Pilot", "Explorer", "Voyager", "MKII", "Delta", "Prime", "Ace", "Alpha", "Omega" };

        string prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Length)];
        string suffix = suffixes[UnityEngine.Random.Range(0, suffixes.Length)];

        int uniqueId = PlayerPrefs.GetInt("username_unique_id", 2);
        PlayerPrefs.SetInt("username_unique_id", uniqueId + 1);

        return $"{prefix}{suffix}{uniqueId}";
    }
}
