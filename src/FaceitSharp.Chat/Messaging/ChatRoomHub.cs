namespace FaceitSharp.Chat.Messaging;

using Network;

/// <summary>
/// Represents a hub chat that has been subscribed to
/// </summary>
public interface IChatRoomHub : IMessageSender
{
    /// <summary>
    /// Triggered whenever a stanza is received that targets the hub
    /// </summary>
    IObservable<Stanza> Stanzas { get; }

    /// <summary>
    /// Triggered whenever a message is received that targets the hub
    /// </summary>
    IObservable<IHubReplyMessage> Messages { get; }

    /// <summary>
    /// Refreshes the hub data
    /// </summary>
    Task Refresh();
}

internal class ChatRoomHub(
    IFaceitChat _chat,
    IChatSocket _socket,
    IFaceitChatCacheService _api,
    FaceitHub _hub) : IChatRoomHub
{
    #region Data
    private JID? _hubJid;

    public JID HubJid
    {
        get => _hubJid ??= new("conference.faceit.com", $"hub-{Hub.Guid}-general");
    }

    public FaceitHub Hub { get; set; } = _hub;
    #endregion

    #region Events
    public IObservable<Stanza> Stanzas => _socket.StanzaReceived
        .Where(x => x.To?.GetBareJID() == HubJid.GetBareJID());

    public IObservable<IHubReplyMessage> Messages =>  _chat.Hubs
        .Message.Where(t => t.Hub.Guid == Hub.Guid)
        .Select(t => new ReplyMessage(HubJid, (RoomMessage)t, _chat));
    #endregion

    #region Message Senders
    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _chat.SendGroupMessage(HubJid, message, images, mentions);

    public Task<Message> Send(string message, params UserMention[] mentions)
        => _chat.SendGroupMessage(HubJid, message, mentions);
    #endregion

    public async Task Refresh()
    {
        Hub = await _api.Hub(Hub.Guid.ToString(), true) ?? throw new Exception("Hub not found");
        _hubJid = null;
    }
}