namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Represents message that can be replied to
/// </summary>
public interface IReplyMessage : IMessageSender, IRoomMessage { }

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
    IFaceitChat _chat) : ITeamReplyMessage, IHubReplyMessage
{
    public MessageType MessageType => _original.MessageType;

    public bool LeftSide => _original.LeftSide;

    public string Content => _original.Content;

    public string Type => _original.Type;

    public FaceitUser[] Mentions => _original.Mentions;

    public bool MentionsEveryone => _original.MentionsEveryone;

    public string[] AttachedImages => _original.AttachedImages;

    public bool MentionsCurrentUser => _original.MentionsCurrentUser;

    public FaceitMatch.FaceitTeam Team => _original.Team;

    public FaceitMatch Match => _original.Match;

    public JID From => _original.From;

    public JID To => _original.To;

    public DateTime Timestamp => _original.Timestamp;

    public string? ResourceId => _original.ResourceId;

    public FaceitUser Author => _original.Author;

    public FaceitHub Hub => _original.Hub;

    public Task<Message> Send(string message, params UserMention[] mentions)
        => _chat.SendGroupMessage(_id, message, mentions);

    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _chat.SendGroupMessage(_id, message, images, mentions);

    public override string ToString()
    {
        var msgContent = $"[{Timestamp:yyyy-MM-dd HH:mm:ss zzz}] {Author.Name}: {Content}";
        foreach(var image in AttachedImages)
            msgContent += $"\r\n\t{image}";

        return msgContent;
    }
}
