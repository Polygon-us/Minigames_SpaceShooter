using Nakama;
using System.Threading.Tasks;

namespace Networking.Nakama.Infrastructure.NakamaAdapters
{
    public class NakamaClient : INakamaClient
    {
        private readonly IClient client;
        private INakamaSession session;
        private readonly ILogProvider logger;

        // Constructor que inyecta ILogProvider y crea el cliente Nakama
        public NakamaClient(NakamaConfig nakamaConfig, ILogProvider logProvider)
        {
            client = new Client(nakamaConfig.scheme, nakamaConfig.host, nakamaConfig.port, nakamaConfig.serverKey);
            logger = logProvider;
        }

        /// <summary>
        /// Conecta al servidor de Nakama con las credenciales proporcionadas.
        /// </summary>
        /// <param name="username">Nombre de usuario para autenticar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AuthenticateDeviceAsync(string deviceId, string username)
        {
            try
            {
                ISession session = await client.AuthenticateDeviceAsync(deviceId, username);
                this.session = new NakamaSession(session);
            }
            catch (System.Exception e)
            {
                logger.LogError(e, "Failed to connect");
                throw;
            }
        }

        /// <summary>
        /// Reconecta al servidor de Nakama usando la sesión almacenada.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task ReconnectAsync(string authToken)
        {
            if (session != null)
            {
                try
                {
                    var session = await client.AuthenticateDeviceAsync(authToken);
                    this.session = new NakamaSession(session);
                    logger.LogInfo("Reconnected to Nakama server.");
                }
                catch (System.Exception e)
                {
                    logger.LogError(e, "Failed to reconnect");
                    throw;
                }
            }
            else
            {
                logger.LogWarning("No session to reconnect.");
            }
        }

        /// <summary>
        /// Desconecta del servidor de Nakama.
        /// </summary>
        public void Disconnect()
        {
            session = null;
            logger.LogInfo("Disconnected from Nakama server.");
        }

        /// <summary>
        /// Obtiene la sesión actual.
        /// </summary>
        /// <returns>La sesión actual.</returns>
        public INakamaSession GetSession()
        {
            return session;
        }

        public IClient GetInstance()
        {
            return client;
        }

        /// <summary>
        /// Verifica si el cliente está conectado.
        /// </summary>
        /// <returns>True si está conectado, de lo contrario false.</returns>
        public bool IsConnected()
        {
            bool isConnected = session != null;
            return isConnected;
        }

        public override string ToString()
        {
            return $"NakamaClient Details:\n" +
                   $"Client Info: {client.ToString()}\n" + // Asegúrate de que IClient tenga un ToString() adecuado.
                   $"Session Info: {session?.ToString() ?? "No active session"}\n" +
                   $"Is Connected: {IsConnected()}\n";
        }

        public Task ReconnectAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
