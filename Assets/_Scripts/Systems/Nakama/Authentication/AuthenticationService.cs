public class AuthenticationService
{
    private NakamaSessionManager sessionManager;

    public AuthenticationService(NakamaSessionManager session)
    {
        sessionManager = session;
    }

    public async System.Threading.Tasks.Task Authenticate()
    {
        await sessionManager.AuthenticateDeviceAsync();
    }
}
