namespace FaceitSharp.Chat.Modules;

/// <summary>
/// Represents a module that handles authentication and keeping track of the current user
/// </summary>
public interface IAuthenticationModule
{
    /// <summary>
    /// Whether or not chat server is logged in
    /// </summary>
    bool LoggedIn { get; }

    /// <summary>
    /// The profile of the currently logged in user
    /// </summary>
    FaceitUserMe Profile { get; }

    /// <summary>
    /// The internal resource identifier for the user
    /// </summary>
    JID Jid { get; }

    /// <summary>
    /// Short hand for `Profile.UserId`
    /// </summary>
    string Id => Profile.UserId;
}

internal class AuthenticationModule(
    ILogger _logger,
    FaceitChatClient _client,
    IResourceIdService _resourceId) : ChatModule(_logger, _client), IAuthenticationModule
{
    private CancellationTokenSource _pingSource = new();
    private CancellationToken? _pinging;
    private FaceitUserMe? _currentUser = null;
    private JID? _jid = null;
    private bool _hasLoggedIn = false;

    public override string ModuleName => "Authentication Module";

    public FaceitUserMe Profile => _currentUser 
        ?? throw new InvalidOperationException("User is not logged in");

    public JID Jid => _jid 
        ?? throw new InvalidOperationException("User is not logged in");

    public bool LoggedIn => _hasLoggedIn 
        && _client.InternalConnection.Connected 
        && _currentUser is not null;

    public async Task<(string userId, string token)> GetLoginInfo()
    {
        var token = await _client.Config.Internal.Token() 
            ?? throw new Exception("Failed to get token");

        _currentUser ??= await _client.Cache.Me()
            ?? throw new Exception("Failed to get user");

        return (_currentUser.UserId, token);
    }

    public async Task<bool> Login(bool reset = false)
    {
        var chat = _client.Connection;
        if (reset)
        {
            _pingSource.Cancel();
            _pingSource = new();
            _hasLoggedIn = false;
        }

        if (LoggedIn)
        {
            Debug("Already logged in");
            return true;
        }

        if (!chat.Connected)
        {
            Warning("Connection is not established");
            return false;
        }

        var (userId, token) = await GetLoginInfo();

        var open = await chat.Send(Open.Create());
        if (open is null)
        {
            Warning("Connection attempt failed");
            return false;
        }

        var auth = Auth.Login(userId, token);
        var result = await chat.Send(auth);
        if (result is null)
        {
            Warning("Failed to authenticate - Request timed out");
            return false;
        }

        if (result is not Auth.Success)
        {
            Warning("Failed to authenticate - Invalid credentials");
            return false;
        }

        open = await chat.Send(Open.Create());
        if (open is null)
        {
            Warning("Failed to open authenticated communication channel with FaceIT");
            return false;
        }

        var bindAuth = BindAuth2.Create(_resourceId.ResourceId());
        result = await chat.Send(bindAuth);
        if (result is null)
        {
            Warning("Failed to bind resource id");
            return false;
        }

        _jid = bindAuth.ProcessResponse(result);

        var bindSession = SessionAuth2.Create();
        result = await chat.Send(bindSession);
        if (result is null)
        {
            Warning("Failed to bind session");
            return false;
        }

        _hasLoggedIn = true;
        return true;
    }

    public void Logout()
    {
        _currentUser = null;
        _jid = null;
        _pingSource.Cancel();
        _pingSource = new();
        _pinging = null;
        _hasLoggedIn = false;
    }

    public override Task OnConnected() => OnReconnected();

    public override Task OnReconnected()
    {
        if (_pinging.HasValue && !_pinging.Value.IsCancellationRequested)
            return Task.CompletedTask;

        _pinging = _pingSource.Token;
        _ = Task.Run(async () =>
        {
            try
            {
                while (_pinging.HasValue 
                    && !_pinging.Value.IsCancellationRequested 
                    && LoggedIn)
                {
                    await Task.Delay(Config.Chat.PingInterval, _pinging.Value);

                    var id = _resourceId.Next();
                    var result = await _client.Connection.Send(Ping.Create(id));
                    if (result is null)
                    {
                        Warning("Failed to ping chat server");
                        break;
                    }

                    Debug("Faceit server ping worked!");
                }
            }
            catch (OperationCanceledException)
            {
                Debug("Ping thread was cancelled");
            }

            _pinging = null;
        }, _pinging.Value);
        return Task.CompletedTask;
    }

    public override Task OnCleanup()
    {
        _pingSource.Cancel();
        return base.OnCleanup();
    }
}
