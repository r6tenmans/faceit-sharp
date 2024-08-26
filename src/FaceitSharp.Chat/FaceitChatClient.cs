namespace FaceitSharp.Chat;

using Modules;
using Modules.Networking;

/// <summary>
/// An instance of the FaceIT chat client
/// </summary>
public interface IFaceitChatClient : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Indicates that the client is connected, authenticated, and ready to use
    /// </summary>
    bool Ready { get; }

    /// <summary>
    /// The config for the client
    /// </summary>
    FaceitConfig Config { get; }

    /// <summary>
    /// Anything related to the underlying connection
    /// </summary>
    IXMPPSocketModule Connection { get; }

    /// <summary>
    /// A caching service for API requests related to the chat client
    /// </summary>
    IApiCacheModule Cache { get; }

    /// <summary>
    /// The service for handling the user authentication
    /// </summary>
    IAuthenticationModule Auth { get; }

    /// <summary>
    /// Anything related to message events
    /// </summary>
    IMessageModule Messages { get; }

    #region Module Stuff
    /// <summary>
    /// All of the <see cref="IChatModule"/>s that are part of the base FaceIT client
    /// </summary>
    IEnumerable<IChatModule> ModulesBase { get; }

    /// <summary>
    /// All of the custom <see cref="IChatModule"/>s that have been added to the client
    /// </summary>
    IEnumerable<IChatModule> ModulesCustom { get; }

    /// <summary>
    /// All of the <see cref="IChatModule"/>s that are part of the client
    /// </summary>
    IEnumerable<IChatModule> Modules { get; }

    /// <summary>
    /// Adds the given module to the client
    /// </summary>
    /// <param name="module">The module to be added</param>
    /// <returns>The current client for chaining</returns>
    /// <remarks>
    /// If the client has already been setup, the <see cref="IChatModule.OnSetup"/> method will be called immediately and awaited
    /// If the client is already connected, the <see cref="IChatModule.OnConnected"/> method will be called immediately and awaited
    /// </remarks>
    IFaceitChatClient AddModule(IChatModule module);

    /// <summary>
    /// Adds a factory for creating a module to the client
    /// </summary>
    /// <param name="bob">The configuration method for creating the module</param>
    /// <returns>The current client for chaining</returns>
    /// <remarks>
    /// If the client has already been setup, the <see cref="IChatModule.OnSetup"/> method will be called immediately and awaited
    /// If the client is already connected, the <see cref="IChatModule.OnConnected"/> method will be called immediately and awaited
    /// </remarks>
    IFaceitChatClient AddModule(Func<IFaceitChatClient, ILogger, FaceitConfig, IChatModule> bob);
    #endregion

    /// <summary>
    /// Connect to the server and attempt to login
    /// </summary>
    /// <returns>Whether or not the login attempt was successful</returns>
    /// <remarks>
    /// This checks if the current connection is active and if the user is logged in before attempting to login
    /// So feel free to call it as many times as you want.
    /// </remarks>
    Task<bool> Login();

    /// <summary>
    /// Disconnects and logs out of the chat client
    /// </summary>
    Task Disconnect();
}

internal class FaceitChatClient(
    FaceitConfig _config,
    IResourceIdService _resourceId,
    IFaceitInternalApiService _api,
    ILogger<FaceitChatClient> _logger) : IFaceitChatClient
{
    private bool _hasRunSetup = false;
    private bool _hasRunConnected = false;
    private readonly List<IChatModule> _customModules = [];
    public readonly List<IDisposable> _disposables = [];

    public FaceitConfig Config => _config;

    public bool Ready => Connection.Connected && Auth.LoggedIn;

    #region Modules
    #region Socket Connection
    private XMPPSocketModule? _internalConnection;
    public XMPPSocketModule InternalConnection => _internalConnection ??= new(_logger, this);
    public IXMPPSocketModule Connection => InternalConnection;
    #endregion

    #region API Cache
    private ApiCacheModule? _internalCache;
    public ApiCacheModule InternalCache => _internalCache ??= new(_logger, this, _api);
    public IApiCacheModule Cache => InternalCache;
    #endregion

    #region Authentication
    private AuthenticationModule? _internalAuth;
    public AuthenticationModule InternalAuth => _internalAuth ??= new(_logger, this, _resourceId);
    public IAuthenticationModule Auth => InternalAuth;
    #endregion

    #region Messaging
    private MessageModule? _internalMessage;
    public MessageModule InternalMessage => _internalMessage ??= new(_logger, this, _resourceId);
    public IMessageModule Messages => InternalMessage;
    #endregion

    public IEnumerable<IChatModule> Modules => ModulesBase.Concat(ModulesCustom);
    public IEnumerable<IChatModule> ModulesBase => [InternalConnection, InternalCache, InternalAuth, InternalMessage];
    public IEnumerable<IChatModule> ModulesCustom => _customModules;
    #endregion

    public async Task<bool> Login()
    {
        await Setup();

        if (!Connection.Connected)
        {
            InternalAuth.Logout();
            var worked = await InternalConnection.Connect();
            if (!worked) return false;
        }

        if (!Auth.LoggedIn)
        {
            var worked = await InternalAuth.Login();
            if (!worked) return false;

            await Connected();
        }

        return true;
    }

    public async Task Disconnect()
    {
        InternalAuth.Logout();
        await InternalConnection.Disconnect();
    }

    public async Task Setup()
    {
        if (_hasRunSetup) return;

        _hasRunSetup = true;
        await Task.WhenAll(Modules.Select(x => x.OnSetup()));

        //When reconnected, ensure we re-login
        Manage(Connection
            .ConnectionReestablished
            .Subscribe(async t =>
            {
                if (await InternalAuth.Login(true))
                    await Reconnected();
            }));
    }

    public async Task Connected()
    {
        if (_hasRunConnected) return;

        _hasRunConnected = true;
        await Task.WhenAll(Modules.Select(x => x.OnConnected()));
    }

    public Task Reconnected() => Task.WhenAll(Modules.Select(x => x.OnReconnected()));

    public IFaceitChatClient AddModule(IChatModule module)
    {
        _customModules.Add(module);

        if (_hasRunSetup) module.OnSetup().Wait();
        if (_hasRunConnected) module.OnConnected().Wait();

        return this;
    }

    public IFaceitChatClient AddModule(Func<IFaceitChatClient, ILogger, FaceitConfig, IChatModule> bob)
    {
        var module = bob(this, _logger, _config);
        return AddModule(module);
    }

    public virtual T Manage<T>(T disposable) where T : IDisposable
    {
        _disposables.Add(disposable);
        return disposable;
    }

    public async ValueTask DisposeAsync()
    {
        await Task.WhenAll(Modules.Select(x => x.OnCleanup()));
        _disposables.ForEach(x => x.Dispose());
        _disposables.Clear();
        _customModules.Clear();

        _internalConnection = null;

        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().Wait();
        GC.SuppressFinalize(this);
    }
}
