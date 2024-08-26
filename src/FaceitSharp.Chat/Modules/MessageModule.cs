namespace FaceitSharp.Chat.Modules;

using Messaging;
using Messaging.Rooms;

/// <summary>
/// Represents a module that handles message parsing and sending
/// </summary>
public interface IMessageModule
{
    /// <summary>
    /// Triggered whenever a message stanza is received
    /// </summary>
    IObservable<Message> Stanzas { get; }

    /// <summary>
    /// Triggered whenever a message event is received
    /// </summary>
    IObservable<IMessageEvent> Events { get; }

    /// <summary>
    /// Triggered whenever a message is received
    /// </summary>
    IObservable<IReplyMessage> All { get; }

    /// <summary>
    /// Triggered whenever a composing event is received
    /// </summary>
    IObservable<IComposing> Composing { get; }

    /// <summary>
    /// Triggered whenever a join announcement is received
    /// </summary>
    IObservable<IJoinAnnouncement> JoinAnnouncements { get; }

    /// <summary>
    /// All messages sent in hub chats (must be subscribed to)
    /// </summary>
    IObservable<IHubReplyMessage> FromHub { get; }

    /// <summary>
    /// All messages sent in match chats (must be subscribed to)
    /// </summary>
    IObservable<IMatchReplyMessage> FromMatch { get; }

    /// <summary>
    /// All messages sent in team chats (must be subscribed to)
    /// </summary>
    IObservable<ITeamReplyMessage> FromTeam { get; }

    /// <summary>
    /// Send a message to the given target
    /// </summary>
    /// <param name="to">Who to send the message to</param>
    /// <param name="message">The message to send</param>
    /// <param name="images">The image IDs to send</param>
    /// <param name="mentions">The users mentioned in the message</param>
    /// <param name="type">The type of message to send</param>
    /// <returns>The message response</returns>
    Task<Message> Send(JID to, string message, string[]? images = null, UserMention[]? mentions = null, string? type = null);

    /// <summary>
    /// Subscribe to a chat room
    /// </summary>
    /// <param name="to">What to subscribe to</param>
    /// <param name="presenceInit">Whether to subscribe to initial presences</param>
    /// <param name="presenceUpdate">Whether to subscribe to presence updates</param>
    /// <param name="priority">The subscription priority</param>
    /// <returns>Whether or not the subscription was successful</returns>
    Task<bool> Subscribe(JID to, bool? presenceInit = null, bool? presenceUpdate = null, int? priority = null);

    /// <summary>
    /// Subscribe to the match and team chats
    /// </summary>
    /// <param name="match">The match to subscribe to</param>
    /// <returns>Whether the subscription was successful or not</returns>
    Task<bool> Subscribe(FaceitMatch match);

    /// <summary>
    /// Subscribes to the hub chat
    /// </summary>
    /// <param name="hub">The hub to subscribe to</param>
    /// <returns>Whether the subscription was successful or not</returns>
    Task<bool> Subscribe(FaceitHub hub);

    /// <summary>
    /// Gets a match room and subscribes to the chats
    /// </summary>
    /// <param name="id">The match ID</param>
    /// <returns>The match room</returns>
    Task<IMatchRoom> MatchRoom(string id);

    /// <summary>
    /// Gets a hub chat and subscribes to the chat
    /// </summary>
    /// <param name="id">The ID of the hub</param>
    /// <returns>The hub chat</returns>
    Task<IHubRoom> HubChat(string id);

    /// <summary>
    /// Indicates a specific message is expected
    /// </summary>
    /// <typeparam name="T">The type of message event</typeparam>
    /// <param name="expectation">The expectation</param>
    /// <param name="timeoutSec">The number of seconds to wait before timing out</param>
    /// <returns>The message that was received</returns>
    Task<T> Expect<T>(MessageExpectation<T> expectation, double timeoutSec) where T : IMessageEvent;

    /// <summary>
    /// Indicates a specific message is expected
    /// </summary>
    /// <typeparam name="T">The type of message event</typeparam>
    /// <param name="configure">The expectation</param>
    /// <param name="timeoutSec">The number of seconds to wait before timing out</param>
    /// <returns>The message that was received</returns>
    Task<T> Expect<T>(Action<MessageExpectation<T>> configure, double timeoutSec) where T : IMessageEvent;
}

internal class MessageModule(
    ILogger _logger,
    FaceitChatClient _client,
    IResourceIdService _resourceId) : ChatModule(_logger, _client), IMessageModule
{
    private readonly ConcurrentDictionary<string, IMatchRoom> _roomMatches = [];
    private readonly ConcurrentDictionary<string, IHubRoom> _roomHubs = [];
    private readonly ConcurrentDictionary<IMessageExpectation, TaskCompletionSource<IMessageEvent>> _resolvers = [];

    public override string ModuleName => "Messaging Module";

    #region Events
    public IObservable<Message> Stanzas => _client.Connection.Stanzas.OfType<Message>();

    public IObservable<IMessageEvent> Events => Stanzas
        .SelectMany(t => ParseMessage(t).ToObservable())
        .Where(t => t.Author.UserId != _client.Auth.Id)
        .Where(t => !MessageHandler(t));

    public IObservable<IComposing> Composing => Events.OfType<IComposing>();

    public IObservable<IJoinAnnouncement> JoinAnnouncements => Events.OfType<IJoinAnnouncement>();

    public IObservable<IReplyMessage> All => Events
        .OfType<RoomMessage>()
        .Select(Contextualize)
        .Where(t => t is not null)
        .Select(t => t!);

    public IObservable<IHubReplyMessage> FromHub => All
        .Where(t => t.Context == ContextType.Hub)
        .Cast<IHubReplyMessage>();

    public IObservable<IMatchReplyMessage> FromMatch => All
        .Where(t => t.Context == ContextType.Match)
        .Cast<IMatchReplyMessage>();

    public IObservable<ITeamReplyMessage> FromTeam => All
        .Where(t => t.Context == ContextType.Team)
        .Cast<ITeamReplyMessage>();
    #endregion

    public async Task<IMatchRoom> MatchRoom(string id)
    {
        if (_roomMatches.TryGetValue(id, out var room))
            return room;

        var match = await _client.Cache.Match(id)
            ?? throw new Exception("Match not found: " + id);

        if (!await Subscribe(match)) 
            throw new Exception("Failed to subscribe to match or team chat room: " + id);

        var matchRoom = new MatchRoom(match, _client);
        _roomMatches.TryAdd(id, matchRoom);        
        return matchRoom;
    }

    public async Task<IHubRoom> HubChat(string id)
    {
        if (_roomHubs.TryGetValue(id, out var room))
            return room;

        var hub = await _client.Cache.Hub(id)
            ?? throw new Exception("Hub not found: " + id);
        if (!await Subscribe(hub))
            throw new Exception("Failed to subscribe to hub chat room: " + id);

        var hubRoom = new HubRoom(hub, _client);
        _roomHubs.TryAdd(id, hubRoom);
        return hubRoom;
    }

    public async Task<Message> Send(JID to, string message, string[]? images = null, UserMention[]? mentions = null, string? type = null)
    {
        var stanza = new SendChat
        {
            To = to.GetBareJID(),
            From = _client.Auth.Jid,
            Body = message,
            ImageIds = images ?? [],
            Mentions = message.ParseMentions(mentions ?? []).ToArray(),
            Id = _resourceId.Next().ToString(),
            Type = type ?? "groupchat"
        };

        return (Message)await _client.Connection.Send(stanza);
    }

    public async Task<bool> Subscribe(JID to, bool? presenceInit = null, bool? presenceUpdate = null, int? priority = null)
    {
        var subscription = ChatSubscription.Create(to, _client.Auth.Jid, presenceInit, presenceUpdate, priority);
        return await _client.Connection.Send(subscription) is not null;
    }

    public async Task<bool> Subscribe(FaceitMatch match)
    {
        var (id, lid, rid) = match.GetJIDs();
        JID[] ids = [id, lid, rid];
        var results = await Task.WhenAll(ids.Select(t => Subscribe(t)));
        return results.All(t => t);
    }

    public Task<bool> Subscribe(FaceitHub hub)
    {
        return Subscribe(hub.GetJID());
    }

    public async Task<T> Expect<T>(MessageExpectation<T> expectation, double timeoutSec)
        where T : IMessageEvent
    {
        var tsc = new TaskCompletionSource<IMessageEvent>();
        _resolvers.TryAdd(expectation, tsc);
        var timeout = TimeSpan.FromSeconds(timeoutSec);

        var ct = new CancellationTokenSource(timeout);
        ct.Token.Register(() =>
        {
            if (_resolvers.TryRemove(expectation, out var tsc))
                tsc.TrySetCanceled();
        }, useSynchronizationContext: false);

        var result = await tsc.Task;
        return (T)result;
    }

    public Task<T> Expect<T>(Action<MessageExpectation<T>> configure, double timeoutSec)
        where T : IMessageEvent
    {
        var expectation = new MessageExpectation<T>();
        configure(expectation);
        return Expect(expectation, timeoutSec);
    }

    public async Task<(FaceitHub?, FaceitMatch?, FaceitTeam?, FaceitUser?)> GetData(ParsedContext context)
    {
        var matchTask = _client.Cache.Match(context.MatchId);
        var hubTask = _client.Cache.Hub(context.HubId);
        var userTask = _client.Cache.User(context.UserId);

        await Task.WhenAll(matchTask, hubTask, userTask);

        var team = matchTask.Result?.Teams.Values.FirstOrDefault(x => x.Id == context.TeamId);

        return (hubTask.Result, matchTask.Result, team, userTask.Result);
    }

    public async Task<FaceitUser[]> ResolveMentions(IEnumerable<Message.Reference> mentions)
    {
        return (await Task.WhenAll(mentions
            .Where(t => t.IsMention && !t.IsEveryone)
            .Select(async m =>
            {
                var uid = m.Uri?.Replace("xmpp:", "").Replace("@faceit.com", "");
                if (string.IsNullOrEmpty(uid)) return null;
                return await _client.Cache.User(uid);
            })))
            .Where(t => t is not null)
            .Select(t => t!)
            .ToArray();
    }

    public async IAsyncEnumerable<IMessageEvent> ParseMessage(Message stanza)
    {
        if (stanza.From is null ||
            stanza.From.Node is null ||
            stanza.To is null)
        {
            Warning("MESSAGE MODULE >> PARSE MESSAGE >> No context found: {event}", stanza.Element.ToXmlString());
            yield break;
        }

        var type = stanza.From.Context(
            out string? matchId, out string? teamId, 
            out string? hubId, out string? userId);
        if (type == ContextType.Unknown)
        {
            Warning("MESSAGE MODULE >> PARSE MESSAGE >> Unknown context: {event}", stanza.Element.ToXmlString());
            yield break;
        }

        var context = new ParsedContext(type, matchId, teamId, hubId, userId);
        var timestamp = stanza.Timestamp ?? stanza.Datas.FirstOrDefault()?.Timestamp ?? DateTime.UtcNow;
        var (hub, match, team, user) = await GetData(context);
        var left = match?.Teams.Where(t => t.Key == Constants.CHAT_FACTION_LEFT && t.Value.Id == team?.Id).Any() ?? true;

        var composing = stanza.Composings.FirstOrDefault();
        if (composing is not null)
        {
            if (user is not null) 
                yield return new Composing
            {
                Context = type,
                Author = user,
                Timestamp = timestamp,
                From = stanza.From,
                To = stanza.To,
                ResourceId = stanza.Id,
                Team = team!,
                Hub = hub!,
                Match = match!,
            };

            yield break;
        }

        var joins = stanza.UserJoins.ToArray();
        if (joins.Length > 0)
        {
            var tasks = joins
                .Select(async t =>
                {
                    if (string.IsNullOrEmpty(t.Node)) return null;

                    var user = await _client.Cache.User(t.Node);
                    if (user is null) return null;

                    return new JoinAnnouncement
                    {
                        Author = user,
                        Timestamp = timestamp,
                        From = stanza.From,
                        To = stanza.To,
                        ResourceId = stanza.Id,
                        Team = team!,
                        Hub = hub!,
                        Match = match!,
                        Context = type,
                    };
                });

            var announcements = await Task.WhenAll(tasks);
            foreach (var announcement in announcements)
                if (announcement is not null) 
                    yield return announcement;
            yield break;
        }

        if (user is null) yield break;

        var mentions = await ResolveMentions(stanza.Mentions);
        var everyone = stanza.Mentions.Any(t => t.IsEveryone);

        yield return new RoomMessage
        {

            Author = user,
            Timestamp = timestamp,
            From = stanza.From,
            To = stanza.To,
            Context = type,
            ResourceId = stanza.Id,
            LeftSide = left,
            Content = stanza.StrBody ?? string.Empty,
            Mentions = mentions,
            MentionsEveryone = everyone,
            MentionsCurrentUser = mentions.Any(t => t.UserId == _client.Auth.Id),
            Type = stanza.Type ?? string.Empty,
            AttachedImages = stanza
                .Images
                .Select(t => t.Src)
                .Where(t => !string.IsNullOrEmpty(t))
                .Select(t => t!)
                .ToArray(),
            Team = team!,
            Hub = hub!,
            Match = match!,
        };
    }

    public IReplyMessage? Contextualize(RoomMessage message)
    {
        var jid = message.Context switch
        {
            ContextType.Match => message.Match.GetJID(),
            ContextType.Team => message.Team.GetJID(message.Match),
            ContextType.Hub => message.Hub.GetJID(),
            _ => null
        };

        if (jid is null) return null;

        return new ReplyMessage(jid, message, _client);
    }

    public bool MessageHandler(IMessageEvent message)
    {
        var handled = false;
        Box(() =>
        {
            var local = _resolvers.ToArray();
            foreach (var (expectation, tsc) in local)
            {
                if (!expectation.Matches(message)) continue;

                handled = true;
                _resolvers.TryRemove(expectation, out _);
                tsc.TrySetResult(message);
            }
        }, "MESSAGE RECEIVED >> RESPONSE PARSER >> {message}", message.ToString());
        return handled;
    }

    public override Task OnSetup()
    {
        Manage(Events.Subscribe());
        return base.OnSetup();
    }

    public override async Task OnReconnected()
    {
        foreach(var (_, item) in _roomMatches)
            await Task.WhenAll(item.Ids.Select(t => Subscribe(t)));

        foreach(var (_, item) in _roomHubs)
            await Subscribe(item.Id);
    }

    public override Task OnCleanup()
    {
        foreach (var (_, tsc) in _resolvers)
            tsc.TrySetCanceled();
        _resolvers.Clear();
        _roomMatches.Clear();
        _roomHubs.Clear();
        return base.OnCleanup();
    }
}

internal record class ParsedContext(
    ContextType Type,
    string? MatchId,
    string? TeamId,
    string? HubId,
    string? UserId);
