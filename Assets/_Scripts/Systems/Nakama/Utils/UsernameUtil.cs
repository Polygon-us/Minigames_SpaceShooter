using UnityEngine;

public static class UsernameUtil
{
    private static readonly string[] Prefixes =
    {
        "Astro", "Nova", "Star", "Galaxy",
        "Nebula", "Comet", "Orion", "Quantum",
        "Photon", "Pulsar", "Cosmic", "Solar",
        "Rocket", "Void"
    };

    private static readonly string[] Suffixes =
    {
        "Commander", "Pilot", "Explorer", "Voyager",
        "MKII", "Delta", "Prime", "Ace",
        "Alpha", "Omega"
    };

    /// <summary>
    /// Genera un nombre de usuario único combinando un prefijo, un sufijo y un ID único.
    /// </summary>
    /// <returns>Un nombre de usuario único como una cadena.</returns>
    public static string GenerateUniqueUsername(string devideId)
    {
        // Elegir un prefijo y un sufijo aleatorios
        string prefix = Prefixes[Random.Range(0, Prefixes.Length)];
        string suffix = Suffixes[Random.Range(0, Suffixes.Length)];

        // Obtener el ID único del PlayerPrefs y aumentarlo en 1
        int uniqueId = PlayerPrefs.GetInt(devideId, 2);
        PlayerPrefs.SetInt(devideId, uniqueId + 1);

        // Retornar el nombre de usuario generado
        return $"{prefix}{suffix}{uniqueId}";
    }
}
