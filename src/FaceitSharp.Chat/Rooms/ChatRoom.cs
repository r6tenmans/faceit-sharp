namespace FaceitSharp.Chat.Rooms;

public interface IChatRoom
{
    IObservable<Stanza> RoomStanzas { get; }

    IObservable<Message> MessageStanzas { get; }

    IObservable<Message> MessageReceived { get; }

    Task<Message> Send(string message, params UserMention[] mentions);

    Task<Message> Send(string message, string[]? images = null, params UserMention[] mentions);
}

public abstract class ChatRoom(
    IFaceitChat _chat,
    IChatSocket _socket,
    IResourceIdService _resourceId,
    JID _id) : IChatRoom
{
    public IObservable<Stanza> RoomStanzas => _socket
        .StanzaReceived
        .Where(t =>
            t.From is not null &&
            t.From.GetBareJID() == _id.GetBareJID());

    public IObservable<Message> MessageStanzas => RoomStanzas
        .OfType<Message>();

    public IObservable<Message> MessageReceived => MessageStanzas
        .Where(t => !t.Composings.Any() && _chat.UserId != t.From?.Resource);

    public static IEnumerable<MessageMention> From(string body, UserMention[] mentions)
    {
        foreach(var userMention in mentions)
        {
            var textToFind = $"@{userMention.UserName}";
            var resourceId = $"xmpp:{userMention.UserId}@faceit.com";
            if (userMention is EveryoneMention)
                resourceId = "room:everyone";

            int startIndex = 0;
            int index;
            while((index = body.IndexOf(textToFind, startIndex)) != -1)
            {
                var end = index + textToFind.Length;
                yield return new MessageMention(resourceId, index, end);
                startIndex = end;
            }
        }
    }

    public async Task<Message> Send(string message, params UserMention[] mentions) =>
        await Send(message, null, mentions);

    public async Task<Message> Send(string message, string[]? images = null, params UserMention[] mentions)
    {
        if (!_chat.Ready) throw new Exception("Chat not ready");

        var stanza = new SendGroupChat
        {
            To = _id.GetBareJID(),
            From = _chat.Jid!,
            Body = message,
            Id = _resourceId.Next().ToString(),
            ImageIds = images ?? [],
            Mentions = From(message, mentions ?? []).ToArray()
        };

        return (Message)await _socket.Send(stanza);
    }

    public abstract Task Refresh();
}

public record class UserMention(string UserName, string UserId);

public record class EveryoneMention() : UserMention("everyone", "everyone");