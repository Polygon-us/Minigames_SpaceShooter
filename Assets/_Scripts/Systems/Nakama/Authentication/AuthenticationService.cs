public class AuthenticationService
{
    private NakamaSession sessionManager;

    public AuthenticationService(NakamaSession session)
    {
        sessionManager = session;
    }

    public async System.Threading.Tasks.Task Authenticate()
    {
        await sessionManager.AuthenticateDeviceAsync();
    }
}
