namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// The different contexts an event can be sent in
/// </summary>
public enum ContextType
{
    /// <summary>
    /// Unable to determine the context of the event
    /// </summary>
    Unknown,
    /// <summary>
    /// Event sent in the hub general chat
    /// </summary>
    Hub,
    /// <summary>
    /// Event sent in a match chat
    /// </summary>
    Match,
    /// <summary>
    /// Event sent in a team chat within a match
    /// </summary>
    Team,
}