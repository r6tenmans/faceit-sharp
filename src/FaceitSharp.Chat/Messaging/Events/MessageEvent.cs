namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Base properties for anything sent in a chat room
/// </summary>
public interface IMessageEvent
{
    /// <summary>
    /// The Jabber ID of the sender
    /// </summary>
    JID From { get; }

    /// <summary>
    /// The jabber ID of the recipient
    /// </summary>
    JID To { get; }

    /// <summary>
    /// The timestamp of the message
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// The optional ID of the message
    /// </summary>
    string? ResourceId { get; }

    /// <summary>
    /// The user who sent the message
    /// </summary>
    FaceitUser Author { get; }

    /// <summary>
    /// Indicates the type of context the message was sent in
    /// </summary>
    /// <remarks>
    /// <see cref="ContextType.Hub"/> will result in a <see cref="IHubMessage"/>
    /// <see cref="ContextType.Match"/> will result in a <see cref="IMatchMessage"/>
    /// <see cref="ContextType.Team"/> will result in a <see cref="ITeamMessage"/>
    /// </remarks>
    ContextType Context { get; }
}

/// <summary>
/// Indicates that the room event was sent in a match
/// </summary>
public interface IMatchEvent : IMessageEvent
{
    /// <summary>
    /// The match that the message was sent in
    /// </summary>
    FaceitMatch Match { get; }
}

/// <summary>
/// Indicates that the room event was sent in a team chat
/// </summary>
public interface ITeamEvent : IMatchEvent
{
    /// <summary>
    /// The team that the message was sent to
    /// </summary>
    FaceitTeam Team { get; }
}

/// <summary>
/// Indicates that the room event was sent in a hub chat
/// </summary>
public interface IHubEvent : IMessageEvent
{
    /// <summary>
    /// The hub the message was sent in
    /// </summary>
    FaceitHub Hub { get; }
}

internal abstract class MessageEvent : ITeamEvent, IHubEvent
{
    private FaceitHub? _hub;
    private FaceitMatch? _match;
    private FaceitTeam? _team;

    public virtual required JID From { get; init; }

    public virtual required JID To { get; init; }

    public virtual required DateTime Timestamp { get; init; }

    public virtual required FaceitUser Author { get; init; }

    public virtual required string? ResourceId { get; init; }

    public virtual required ContextType Context { get; init; }

    public virtual FaceitMatch Match
    {
        get => _match ?? throw new InvalidOperationException("Match is not set");
        set => _match = value;
    }

    public virtual FaceitTeam Team
    {
        get => _team ?? throw new InvalidOperationException("Team is not set");
        set => _team = value;
    }

    public virtual FaceitHub Hub
    {
        get => _hub ?? throw new InvalidOperationException("Hub is not set");
        set => _hub = value;
    }
}