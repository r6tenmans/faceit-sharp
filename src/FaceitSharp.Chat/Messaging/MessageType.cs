namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// The different contexts a message can be sent in
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Message sent in the hub general chat
    /// </summary>
    Hub = 0,
    /// <summary>
    /// Message sent in a match chat
    /// </summary>
    Match = 1,
    /// <summary>
    /// Message sent in a team chat within a match
    /// </summary>
    Team = 2,
}
