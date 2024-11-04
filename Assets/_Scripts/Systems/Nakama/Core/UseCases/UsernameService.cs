using System;
using System.Text;
using UnityEngine;

namespace YourNamespace.Core.Services
{
    /// <summary>
    /// Service for generating unique usernames and display names.
    /// </summary>
    public class UsernameService
    {
        private const string ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Generates a unique username with the specified length.
        /// </summary>
        /// <returns>A unique username.</returns>
        public string GenerateUniqueUsername()
        {
            string username;
            do
            {
                username = GenerateRandomString(16);
            } while (!IsUsernameUnique(username)); // Implementar verificación de unicidad

            return username;
        }

        /// <summary>
        /// Generates a display name that starts with "GUEST_000" and ends with a unique 3-digit number.
        /// </summary>
        /// <returns>A display name.</returns>
        public string GenerateDisplayName()
        {
            string uniqueSuffix = UnityEngine.Random.Range(100, 999).ToString();
            return $"GUEST_000{uniqueSuffix}";
        }

        private string GenerateRandomString(int length)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(ValidCharacters[UnityEngine.Random.Range(0, ValidCharacters.Length)]);
            }
            return result.ToString();
        }

        private bool IsUsernameUnique(string username)
        {
            // Implementar lógica para verificar la unicidad del nombre de usuario en el servidor
            return true;
        }
    }
}
