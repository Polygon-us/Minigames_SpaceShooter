using UnityEngine;

public static class DeviceIdUtil
{
    private const string DeviceIdKey = "deviceId";

    /// <summary>
    /// Obtiene el Device ID único del jugador. Si no existe, genera uno nuevo.
    /// </summary>
    /// <returns>El Device ID único como una cadena.</returns>
    public static string GetDeviceId()
    {
        // Obtener el Device ID almacenado en PlayerPrefs, o usar el identificador único del dispositivo
        string deviceId = PlayerPrefs.GetString(DeviceIdKey, SystemInfo.deviceUniqueIdentifier);

        // Comprobar si el identificador es el no soportado
        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            // Generar un nuevo GUID como el Device ID
            deviceId = System.Guid.NewGuid().ToString();
            // Almacenar el nuevo Device ID en PlayerPrefs
            PlayerPrefs.SetString(DeviceIdKey, deviceId);
        }

        return deviceId;
    }
}
