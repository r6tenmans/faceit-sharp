namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Base properties for anything sent in a chat room
/// </summary>
public interface IRoom
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
}

/// <summary>
/// Indicates that the room event was sent in a match
/// </summary>
public interface IRoomMatch : IRoom
{
    /// <summary>
    /// The match that the message was sent in
    /// </summary>
    FaceitMatch Match { get; }
}

/// <summary>
/// Indicates that the room event was sent in a team chat
/// </summary>
public interface IRoomTeam : IRoomMatch
{
    /// <summary>
    /// The team that the message was sent to
    /// </summary>
    FaceitMatch.FaceitTeam Team { get; }
}

/// <summary>
/// Indicates that the room event was sent in a hub chat
/// </summary>
public interface IRoomHub : IRoom
{
    /// <summary>
    /// The hub the message was sent in
    /// </summary>
    FaceitHub Hub { get; }
}

internal abstract class Room : IRoomTeam, IRoomHub
{
    private FaceitHub? _hub;
    private FaceitMatch? _match;
    private FaceitMatch.FaceitTeam? _team;

    public virtual required JID From { get; init; }

    public virtual required JID To { get; init; }

    public virtual required DateTime Timestamp { get; init; }

    public virtual required FaceitUser Author { get; init; }

    public virtual required string? ResourceId { get; init; }

    public virtual FaceitMatch Match
    {
        get => _match ?? throw new InvalidOperationException("Match is not set");
        set => _match = value;
    }

    public virtual FaceitMatch.FaceitTeam Team
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