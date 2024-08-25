namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Represents the base properties for a message that was sent in a chat room.
/// </summary>
public interface IRoomMessage : IRoom
{
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
    /// Any images that are attached to the message
    /// </summary>
    string[] AttachedImages { get; }

    /// <summary>
    /// Indicates whether or not the message mentions the current user
    /// </summary>
    bool MentionsCurrentUser { get; }

    /// <summary>
    /// Indicates the type of context the message was sent in
    /// </summary>
    /// <remarks>
    /// <see cref="MessageType.Hub"/> will result in a <see cref="IHubMessage"/>
    /// <see cref="MessageType.Match"/> will result in a <see cref="IMatchMessage"/>
    /// <see cref="MessageType.Team"/> will result in a <see cref="ITeamMessage"/>
    /// </remarks>
    MessageType MessageType { get; }
}

/// <summary>
/// Represents a message that was sent in a match room.
/// </summary>
public interface IMatchMessage : IRoomMessage, IRoomMatch { }

/// <summary>
/// Represents a message that was sent in a team chat
/// </summary>
public interface ITeamMessage : IMatchMessage, IRoomTeam 
{
    /// <summary>
    /// Whether or not the message was sent in the left-side (faction1) team chat
    /// </summary>
    bool LeftSide { get; }
}

/// <summary>
/// Represents a message that was sent in a hub chat
/// </summary>
public interface IHubMessage : IRoomMessage, IRoomHub { }

internal class RoomMessage : Room, ITeamMessage, IHubMessage
{
    public virtual required string Content { get; init; }

    public virtual required string Type { get; init; }

    public virtual required FaceitUser[] Mentions { get; init; }

    public virtual required bool MentionsEveryone { get; init; }

    public virtual required bool MentionsCurrentUser { get; init; }

    public virtual required string[] AttachedImages { get; init; }

    public virtual required MessageType MessageType { get; init; }

    public virtual bool LeftSide { get; set; }
}