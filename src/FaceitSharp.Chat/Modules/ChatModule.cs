namespace FaceitSharp.Chat.Modules;

/// <summary>
/// Represents a module that can be added to the client
/// </summary>
public interface IChatModule
{
    /// <summary>
    /// The name of the current module for logging purposes
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Whether to rethrow handled exceptions or just log them.
    /// </summary>
    bool RethrowExceptions { get; }

    /// <summary>
    /// The minimum log level required for this module to log
    /// </summary>
    LogLevel MinimumLogLevel { get; }

    /// <summary>
    /// The parent client that this module is attached to
    /// </summary>
    IFaceitChatClient Parent { get; }

    /// <summary>
    /// Triggered before the client attempts to connect to the server for the first time.
    /// </summary>
    /// <remarks>
    /// Called only once in the lifetime of the <see cref="IFaceitChatClient"/>.
    /// All of the modules have been created at this point, but the client has not connected yet.
    /// You can use this to subscribe to reactive events or do any other setup.
    /// </remarks>
    Task OnSetup();

    /// <summary>
    /// Triggered when the client has connected to the server and authenticated
    /// </summary>
    /// <remarks>Called only once in the lifetime of the <see cref="IFaceitChatClient"/></remarks>
    Task OnConnected();

    /// <summary>
    /// Triggered when the client reconnects to the server
    /// </summary>
    /// <returns>Will be called whenever the socket reconnects to the server</returns>
    Task OnReconnected();

    /// <summary>
    /// Triggered when the <see cref="IFaceitChatClient"/> is disposed
    /// </summary>
    /// <remarks>
    /// Called only once in the lifetime of the <see cref="IFaceitChatClient"/>.
    /// Think of this as <see cref="IDisposable.Dispose"/>
    /// </remarks>
    Task OnCleanup();

    /// <summary>
    /// Logs the given error and rethrows if <see cref="RethrowExceptions"/> is true
    /// </summary>
    /// <param name="error">The exception that was thrown</param>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void HandleException(Exception error, string message, params object?[] args);

    /// <summary>
    /// Encapsulates the given action in a try-catch block and handles any exceptions
    /// </summary>
    /// <param name="action">The action to perform</param>
    /// <param name="context">A brief description of what is happening</param>
    /// <param name="args">Any arguments to include in the error messages</param>
    Task Box(Func<Task> action, string context, params object?[] args);

    /// <summary>
    /// Encapsulates the given action in a try-catch block and handles any exceptions
    /// </summary>
    /// <param name="action">The action to perform</param>
    /// <param name="context">A brief description of what is happening</param>
    /// <param name="args">Any arguments to include in the error messages</param>
    void Box(Action action, string context, params object?[] args);

    /// <summary>
    /// Manages the given disposable and disposes it when the module is disposed
    /// </summary>
    /// <typeparam name="T">The disposable target type</typeparam>
    /// <param name="disposable">The disposable object</param>
    /// <returns>The same disposable object</returns>
    T Manage<T>(T disposable) where T : IDisposable;

    /// <summary>
    /// Log an event to the underlying logger
    /// </summary>
    /// <param name="level">The level of the logger</param>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Log(LogLevel level, string message, params object?[] args);

    /// <summary>
    /// Log an event to the underlying logger
    /// </summary>
    /// <param name="level">The level of the logger</param>
    /// <param name="error">The exception to include in the log</param>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Log(LogLevel level, Exception error, string message, params object?[] args);

    /// <summary>
    /// Writes an trace log to the underlying logger
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Trace(string message, params object?[] args);

    /// <summary>
    /// Writes a debug log to the underlying logger
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Debug(string message, params object?[] args);

    /// <summary>
    /// Writes an info log to the underlying logger
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Info(string message, params object?[] args);

    /// <summary>
    /// Writes a warning log to the underlying logger
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Warning(string message, params object?[] args);

    /// <summary>
    /// Writes an info log to the underlying logger
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Error(string message, params object?[] args);

    /// <summary>
    /// Writes an info log to the underlying logger
    /// </summary>
    /// <param name="error">The exception to log</param>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Error(Exception error, string message, params object?[] args);

    /// <summary>
    /// Writes a critical log to the underlying logger
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Critical(string message, params object?[] args);

    /// <summary>
    /// Writes a critical log to the underlying logger
    /// </summary>
    /// <param name="error">The exception to log</param>
    /// <param name="message">The message to log</param>
    /// <param name="args">The arguments to log</param>
    void Critical(Exception error, string message, params object?[] args);
}

internal abstract class ChatModule(
    ILogger _logger,
    IFaceitChatClient _client) : IChatModule
{
    public FaceitConfig Config => _client.Config;

    public List<IDisposable> Disposables { get; } = [];

    public virtual bool RethrowExceptions => false;

    /// <summary>
    /// This has no effect in production builds. Purely for testing
    /// </summary>
    public virtual bool DebugMode => true;

    public virtual LogLevel MinimumLogLevel
    {
        get
        {
#if DEBUG
            return DebugMode ? LogLevel.Trace : _client.Config.Chat.LogLevel;
#else
            return _client.Config.Chat.LogLevel;
#endif
        }
    }

    public abstract string ModuleName { get; }

    public IFaceitChatClient Parent => _client;

    public virtual Task OnCleanup()
    {
        Disposables.ForEach(x => Box(x.Dispose, "Disposing {type}", x.GetType().Name));
        Disposables.Clear();
        return Task.CompletedTask;
    }

    public virtual Task OnConnected() => Task.CompletedTask;

    public virtual Task OnReconnected() => Task.CompletedTask;

    public virtual Task OnSetup() => Task.CompletedTask;

    public virtual T Manage<T>(T disposable) where T : IDisposable
    {
        Disposables.Add(disposable);
        return disposable;
    }

    #region Error Handling
    public virtual void HandleException(Exception error, string message, params object?[] args)
    {
        Error(error, message, args);
        if (RethrowExceptions) throw error;
    }

    public async Task Box(Func<Task> action, string context, params object?[] args)
    {
        try
        {
            Debug("[BOXED ACTION] STARTING >> " + context, args);
            await action();
            Debug("[BOXED ACTION] FINISHED >> " + context, args);
        }
        catch (Exception error)
        {
            HandleException(error, context, args);
        }
    }

    public void Box(Action action, string context, params object?[] args)
    {
        try
        {
            Debug("[BOXED ACTION] STARTING >> " + context, args);
            action();
            Debug("[BOXED ACTION] FINISHED >> " + context, args);
        }
        catch (Exception error)
        {
            HandleException(error, context, args);
        }
    }
    #endregion

    #region Logging
    public virtual bool MeetsLevel(LogLevel level)
    {
        return MinimumLogLevel != LogLevel.None 
            && level >= MinimumLogLevel
            && _logger.IsEnabled(level);
    }

    public virtual void Log(LogLevel level, string message, params object?[] args)
    {
        Log(level, null, message, args);
    }

    public virtual void Log(LogLevel level, Exception? error, string message, params object?[] args)
    {
        if (!MeetsLevel(level)) return;
        _logger.Log(level, error, $"[{ModuleName}] {message}", args);
    }

    public virtual void Trace(string message, params object?[] args)
    {
        Log(LogLevel.Trace, message, args);
    }

    public virtual void Debug(string message, params object?[] args)
    {
        Log(LogLevel.Debug, message, args);
    }

    public virtual void Info(string message, params object?[] args)
    {
        Log(LogLevel.Information, message, args);
    }

    public virtual void Warning(string message, params object?[] args)
    {
        Log(LogLevel.Warning, message, args);
    }

    public virtual void Error(string message, params object?[] args)
    {
        Log(LogLevel.Error, message, args);
    }

    public virtual void Error(Exception error, string message, params object?[] args)
    {
        Log(LogLevel.Error, error, message, args);
    }

    public virtual void Critical(string message, params object?[] args)
    {
        Log(LogLevel.Critical, message, args);
    }

    public virtual void Critical(Exception error, string message, params object?[] args)
    {
        Log(LogLevel.Critical, error, message, args);
    }
    #endregion
}
