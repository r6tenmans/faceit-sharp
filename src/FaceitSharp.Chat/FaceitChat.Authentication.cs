namespace FaceitSharp.Chat;

public interface IFaceitChatAuth
{
    FaceitUserMe? Current { get; }

    JID? Jid { get; }

    string? UserId { get; }

    bool Authenticated { get; }

    bool Ready { get; }

    int PingIntervalSec { get; set; }

    Task<bool> Login();
}

internal partial class FaceitChat
{
    public FaceitUserMe? Current { get; private set; }

    public bool Authenticated { get; private set; }

    public bool Ready => _chat.Connected && Authenticated;

    public int PingIntervalSec { get; set; } = 30;

    public string? UserId => Current?.UserId;

    public JID? Jid { get; private set; }

    public async Task<(string userId, string token)> GetLoginInfo()
    {
        var token = await _config.InternalApiToken() 
            ?? throw new Exception("Failed to get token");

        Current ??= await _api.Me() 
            ?? throw new Exception("Failed to get user");

        return (Current.UserId, token);
    }

    public async Task<bool> Login()
    {
        if (_chat.Connected && Authenticated) return true;

        var (userId, token) = await GetLoginInfo();
        var auth = Auth.Login(userId, token);

        var connected = await _chat.Connect();
        if (!connected)
        {
            _logger.LogWarning("Connection attempt failed");
            return false;
        }

        var open = await _chat.Send(Open.Create());
        if (open is null)
        {
            _logger.LogWarning("Failed to open communication channel with FaceIT");
            return false;
        }

        var result = await _chat.Send(auth);
        if (result is null)
        {
            _logger.LogWarning("Failed to authenticate with FaceIT - No result found");
            return false;
        }

        if (result is not Auth.Success)
        {
            _logger.LogWarning("Failed to authenticate with FaceIT - Login attempt wasn't successful");
            return false;
        }

        open = await _chat.Send(Open.Create());
        if (open is null)
        {
            _logger.LogWarning("Failed to open authenticated communication channel with FaceIT");
            return false;
        }

        var bindAuth = BindAuth2.Create(_resourceId.ResourceId());
        result = await _chat.Send(bindAuth);
        if (result is null)
        {
            _logger.LogWarning("Failed to bind resource id");
            return false;
        }

        Jid = bindAuth.ProcessResponse(result);

        var bindSession = SessionAuth2.Create();
        result = await _chat.Send(bindSession);
        if (result is null)
        {
            _logger.LogWarning("Failed to bind session");
            return false;
        }

        Authenticated = true;
        _ = Task.Run(Background);
        return true;
    }

    public async Task Background()
    {
        SubscriptionsSetup();
        await PingThread();
    }

    public async Task PingThread()
    {
        if (!Ready)
        {
            _logger.LogWarning("Ping thread abandoned - Not ready");
            return;
        }

        await Task.Delay(PingIntervalSec * 1000);

        var id = _resourceId.Next();
        var result = await _chat.Send(Ping.Create(id));
        if (result is null)
        {
            _logger.LogWarning("Failed to ping server");
            return;
        }

        _logger.LogDebug("Faceit Ping Successful!");
        await PingThread();
    }
}
