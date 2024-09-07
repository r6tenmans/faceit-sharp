namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Represents the base properties for a message that was sent in a chat room.
/// </summary>
public interface IRoomMessage : IMessageEvent
{
    /// <summary>
    /// The date and time the message was edited
    /// </summary>
    /// <remarks>Null if the message hasn't been edited</remarks>
    DateTime? Edited { get; }

    /// <summary>
    /// The ID of the message that was received
    /// </summary>
    string? MessageId { get; }

    /// <summary>
    /// The content of the message.
    /// </summary>
    string Content { get; }

    /// <summary>
    /// The type of message received
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Any users that are mentioned in the message
    /// </summary>
    FaceitUser[] Mentions { get; }

    /// <summary>
    /// Whether or not the message mentions everyone in the room
    /// </summary>
    bool MentionsEveryone { get; }

    /// <summary>
    /// Whether or not the message mentions everyone currently in the room
    /// </summary>
    bool MentionsHere { get; }

    /// <summary>
    /// Any images that are attached to the message
    /// </summary>
    string[] AttachedImages { get; }

    /// <summary>
    /// Indicates whether or not the message mentions the current user
    /// </summary>
    bool MentionsCurrentUser { get; }
}

/// <summary>
/// Represents a message that was sent in a match room.
/// </summary>
public interface IMatchMessage : IRoomMessage, IMatchEvent { }

/// <summary>
/// Represents a message that was sent in a team chat
/// </summary>
public interface ITeamMessage : IMatchMessage, ITeamEvent
{
    /// <summary>
    /// Whether or not the message was sent in the left-side (faction1) team chat
    /// </summary>
    bool LeftSide { get; }
}

/// <summary>
/// Represents a message that was sent in a hub chat
/// </summary>
public interface IHubMessage : IRoomMessage, IHubEvent { }

internal class RoomMessage : MessageEvent, ITeamMessage, IHubMessage
{
    public virtual required string? MessageId { get; init; }

    public virtual required DateTime? Edited { get; init; }

    public virtual required string Content { get; init; }

    public virtual required string Type { get; init; }

    public virtual required FaceitUser[] Mentions { get; init; }

    public virtual required bool MentionsEveryone { get; init; }

    public virtual required bool MentionsHere { get; init; }

    public virtual required bool MentionsCurrentUser { get; init; }

    public virtual required string[] AttachedImages { get; init; }

    public virtual bool LeftSide { get; set; }
}