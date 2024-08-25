namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Represents a service that can send a message
/// </summary>
/// <remarks>Target this for extension methods</remarks>
public interface IMessageSender
{
    /// <summary>
    /// Sends a message to the match chat
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="mentions">Any users to mention</param>
    /// <returns>The result of the message send</returns>
    Task<Message> Send(string message, params UserMention[] mentions);

    /// <summary>
    /// Sends a message to the match chat
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="images">Any images to send</param>
    /// <param name="mentions">Any users to mention</param>
    /// <returns>The result of the message send</returns>
    Task<Message> Send(string message, string[] images, params UserMention[] mentions);
}
