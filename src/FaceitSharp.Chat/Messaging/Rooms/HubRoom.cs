namespace FaceitSharp.Chat.Messaging.Rooms;

/// <summary>
/// Represents a hub chat on FaceIT that can be interacted with
/// </summary>
public interface IHubRoom : IMessageSender
{
    /// <summary>
    /// The information for the hub
    /// </summary>
    FaceitHub Hub { get; }

    /// <summary>
    /// The JID of the hub
    /// </summary>
    JID Id { get; }

    /// <summary>
    /// Messages from the hub chat
    /// </summary>
    IObservable<IHubReplyMessage> Messages { get; }

    /// <summary>
    /// Indicates that a message is being composed in the hub chat
    /// </summary>
    IObservable<IHubComposing> Composing { get; }

    /// <summary>
    /// Indicates that a user joined the hub
    /// </summary>
    IObservable<IHubAnnouncement> JoinAnnouncements { get; }

    /// <summary>
    /// Refreshes the data for the hub
    /// </summary>
    Task Refresh();
}

internal class HubRoom(
    FaceitHub _initial,
    IFaceitChatClient _client) : IHubRoom
{
    private JID? _id;
    private IObservable<IHubReplyMessage>? _messages;
    private IObservable<IHubComposing>? _composing;
    private IObservable<IHubAnnouncement>? _joinAnnouncements;

    public FaceitHub Hub { get; set; } = _initial;

    public JID Id => _id ??= Hub.GetJID();

    public IObservable<IHubReplyMessage> Messages => _messages ??= _client.Messages.FromHub.Where(t => t.Hub.Guid == Hub.Guid);

    public IObservable<IHubComposing> Composing => _composing ??= _client.Messages.Composing
        .Where(t => t.Context == ContextType.Hub)
        .Cast<IHubComposing>()
        .Where(t => t.Hub.Guid == Hub.Guid);

    public IObservable<IHubAnnouncement> JoinAnnouncements => _joinAnnouncements ??= _client.Messages.JoinAnnouncements
        .Where(t => t.Context == ContextType.Hub)
        .Cast<IHubAnnouncement>()
        .Where(t => t.Hub.Guid == Hub.Guid);

    public async Task Refresh()
    {
        var hub = await _client.Cache.Hub(Hub.Guid, true)
            ?? throw new Exception("Hub not found");
        Hub = hub;
        _id = null;
    }

    public Task<Message> Send(string message, params UserMention[] mentions) => Send(message, [], mentions);

    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _client.Messages.Send(Id, message, images, mentions);
}
