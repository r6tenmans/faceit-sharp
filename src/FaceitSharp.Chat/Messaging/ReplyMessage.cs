namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Represents message that can be replied to
/// </summary>
public interface IReplyMessage : IMessageSender, IRoomMessage
{
    /// <summary>
    /// Attempts to delete the message
    /// </summary>
    /// <returns>The delete request response</returns>
    Task<Iq> Delete();
}

/// <summary>
/// Represents a hub message that can be replied to
/// </summary>
public interface IHubReplyMessage : IReplyMessage, IHubMessage { }

/// <summary>
/// Represents a match message that can be replied to
/// </summary>
public interface IMatchReplyMessage : IReplyMessage, IMatchMessage { }

/// <summary>
/// Represents a team message that can be replied to
/// </summary>
public interface ITeamReplyMessage : IMatchReplyMessage, ITeamMessage { }

internal class ReplyMessage(
    JID _id,
    RoomMessage _original,
    IFaceitChatClient _chat) : ITeamReplyMessage, IHubReplyMessage
{
    public string? MessageId => _original.MessageId;

    public DateTime? Edited => _original.Edited;

    public ContextType Context => _original.Context;

    public bool LeftSide => _original.LeftSide;

    public string Content => _original.Content;

    public string Type => _original.Type;

    public FaceitUser[] Mentions => _original.Mentions;

    public bool MentionsEveryone => _original.MentionsEveryone;

    public bool MentionsHere => _original.MentionsHere;

    public string[] AttachedImages => _original.AttachedImages;

    public bool MentionsCurrentUser => _original.MentionsCurrentUser;

    public FaceitTeam Team => _original.Team;

    public FaceitMatch Match => _original.Match;

    public JID From => _original.From;

    public JID To => _original.To;

    public DateTime Timestamp => _original.Timestamp;

    public string? ResourceId => _original.ResourceId;

    public FaceitUser Author => _original.Author;

    public FaceitHub Hub => _original.Hub;

    public Task<Message> Send(string message, params UserMention[] mentions)
        => _chat.Messages.Send(_id, message, [], mentions);

    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _chat.Messages.Send(_id, message, images, mentions);

    public Task<Iq> Delete()
    {
        if (string.IsNullOrEmpty(MessageId))
            throw new Exception("Cannot delete a message without a message id");

        return _chat.Messages.Delete(_id, MessageId);
    }

    public override string ToString()
    {
        var msgContent = $"[{Timestamp:yyyy-MM-dd HH:mm:ss zzz}] {Author.Name}: {Content}";
        foreach (var image in AttachedImages)
            msgContent += $"\r\n\t{image}";

        return msgContent;
    }
}
