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
    /// Mute a user in the chat
    /// </summary>
    /// <param name="user">The user</param>
    /// <param name="duration">The duration to mute for</param>
    /// <returns>The result of the request</returns>
    Task<bool> Mute(FaceitPartialUser user, TimeSpan duration) => Mute(user.UserId, duration);

    /// <summary>
    /// Mute a user in the chat
    /// </summary>
    /// <param name="user">The user's FaceIT ID</param>
    /// <param name="duration">The duration to mute for</param>
    /// <returns>The result of the request</returns>
    Task<bool> Mute(string user, TimeSpan duration);

    /// <summary>
    /// Unmute a user in the chat
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>The result of the request</returns>
    Task<bool> Unmute(FaceitPartialUser user) => Unmute(user.UserId);

    /// <summary>
    /// Unmute a user in the chat
    /// </summary>
    /// <param name="user">The user's FaceIT ID</param>
    /// <returns>The result of the request</returns>
    Task<bool> Unmute(string user);

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

    public Task<bool> Mute(string userId, TimeSpan duration)
        => _client.Messages.Mute(userId, Hub.Guid, duration);

    public Task<bool> Unmute(string userId) => _client.Messages.Unmute(userId, Hub.Guid);

    public Task<Message> Send(string message, params UserMention[] mentions) => Send(message, [], mentions);

    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _client.Messages.Send(Id, message, images, mentions);
}
