namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Indicates that a message has been deleted
/// </summary>
public interface IMessageDeleted : IMessageEvent 
{
    /// <summary>
    /// The ID of the message that was deleted
    /// </summary>
    string MessageId { get; }

    /// <summary>
    /// The optional user who deleted the message
    /// </summary>
    /// <remarks>This can be null if no user object was provided</remarks>
    new FaceitUser? Author { get; }
}

/// <summary>
/// Indicates that a message has been deleted in a hub chat
/// </summary>
public interface IHubMessageDeleted : IHubEvent, IMessageDeleted { }

/// <summary>
/// Indicates that a message has been deleted in a match room
/// </summary>
public interface IMatchMessageDeleted : IMatchEvent, IMessageDeleted { }

/// <summary>
/// Indicates that a message has been deleted in a team chat
/// </summary>
public interface ITeamMessageDeleted : ITeamEvent, IMatchMessageDeleted { }

internal class MessageDeleted : MessageEvent, IHubMessageDeleted, ITeamMessageDeleted
{
    public virtual required string MessageId { get; init; }
}
