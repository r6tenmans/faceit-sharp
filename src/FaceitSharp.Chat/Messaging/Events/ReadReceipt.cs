namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Indicates that a message has been read
/// </summary>
public interface IReadReceipt : IMessageEvent { }

internal class ReadReceipt : MessageEvent, IReadReceipt { }