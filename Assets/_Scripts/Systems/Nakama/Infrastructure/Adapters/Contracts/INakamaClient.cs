using Nakama;
using System.Threading.Tasks;

namespace Networking.Nakama.Infrastructure.NakamaAdapters
{
    public interface INakamaClient
    {
        /// <summary>
        /// Autentica el dispositivo con el servidor de Nakama usando un nombre de usuario.
        /// </summary>
        /// <param name="deviceId">ID del dispositivo para la autenticación.</param>
        /// <param name="username">Nombre de usuario para autenticar.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        Task AuthenticateDeviceAsync(string deviceId, string username);

        /// <summary>
        /// Reconecta al servidor de Nakama usando la sesión almacenada.
        /// </summary>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        Task ReconnectAsync();

        /// <summary>
        /// Desconecta del servidor de Nakama.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Obtiene la sesión actual.
        /// </summary>
        /// <returns>La sesión actual.</returns>
        INakamaSession GetSession();

        IClient GetInstance();

        /// <summary>
        /// Verifica si el cliente está conectado.
        /// </summary>
        /// <returns>True si está conectado, de lo contrario false.</returns>
        bool IsConnected();
    }
}
