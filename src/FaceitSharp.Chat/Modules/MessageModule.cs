namespace FaceitSharp.Chat.Modules;

using Messaging;
using Messaging.Rooms;
using static FaceitSharp.Api.Internal.Models.FaceitTournamentParticipant;
using System.Reactive;
using System.Text.RegularExpressions;

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
    /// All messages that are deleted
    /// </summary>
    IObservable<IMessageDeleted> MessageDeleted { get; }

    /// <summary>
    /// Any message that is edited
    /// </summary>
    IObservable<IReplyMessage> MessageEdited { get; }

    /// <summary>
    /// Any message that was sent
    /// </summary>
    IObservable<Message> MessageSent { get; }

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
    /// Edit a message
    /// </summary>
    /// <param name="to">What the context of the message was</param>
    /// <param name="messageId">The resource ID of the message</param>
    /// <param name="content">The edited message content</param>
    /// <returns>The edit request response</returns>
    Task<Iq> Edit(JID to, string messageId, string content);

    /// <summary>
    /// Edit a message
    /// </summary>
    /// <param name="message">The message to edit</param>
    /// <param name="content">The edited message content</param>
    /// <returns>The edit request response</returns>
    Task<Iq> Edit(IRoomMessage message, string content);

    /// <summary>
    /// Edit a message
    /// </summary>
    /// <param name="message">The message to edit</param>
    /// <param name="content">The edited message content</param>
    /// <returns>The edit request response</returns>
    Task<Iq> Edit(Message message, string content);

    /// <summary>
    /// Delete a message
    /// </summary>
    /// <param name="to">What the context of the message was</param>
    /// <param name="messageId">The resource ID of the message</param>
    /// <returns>The delete request response</returns>
    Task<Iq> Delete(JID to, string messageId);

    /// <summary>
    /// Delete a message
    /// </summary>
    /// <param name="message">The message to delete</param>
    /// <returns>The delete request response</returns>
    Task<Iq> Delete(IRoomMessage message);

    /// <summary>
    /// Delete a message
    /// </summary>
    /// <param name="message">The message to delete</param>
    /// <returns>The delete request response</returns>
    Task<Iq> Delete(Message message);

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

    /// <summary>
    /// Mute a user in a hub chat
    /// </summary>
    /// <param name="userId">The user's FaceIT ID</param>
    /// <param name="hubId">The hub ID to mute in</param>
    /// <param name="duration">How long the mute should last</param>
    /// <returns>The result of the mute</returns>
    Task<bool> Mute(string userId, string hubId, TimeSpan duration);

    /// <summary>
    /// Unmute a user in a hub chat
    /// </summary>
    /// <param name="userId">The user's FaceIT ID</param>
    /// <param name="hubId">The hub ID to mute in</param>
    /// <returns>The result of the unmute</returns>
    Task<bool> Unmute(string userId, string hubId);
}

internal class MessageModule(
    ILogger _logger,
    FaceitChatClient _client,
    IResourceIdService _resourceId) : ChatModule(_logger, _client), IMessageModule
{
    private readonly ConcurrentDictionary<string, IMatchRoom> _roomMatches = [];
    private readonly ConcurrentDictionary<string, IHubRoom> _roomHubs = [];
    private readonly ConcurrentDictionary<IMessageExpectation, TaskCompletionSource<IMessageEvent>> _resolvers = [];
    private readonly Subject<IMessageEvent> _messageEvents = new();
    private readonly Subject<Message> _messageSent = new();

    public override string ModuleName => "Messaging Module";

    #region Events
    private IObservable<Message>? _stanzas;
    private IObservable<IMessageEvent>? _events;
    private IObservable<IComposing>? _composing;
    private IObservable<IJoinAnnouncement>? _joinAnnouncements;
    private IObservable<IReplyMessage>? _all;
    private IObservable<IHubReplyMessage>? _fromHub;
    private IObservable<IMatchReplyMessage>? _fromMatch;
    private IObservable<ITeamReplyMessage>? _fromTeam;
    private IObservable<IMessageDeleted>? _messageDeleted;
    private IObservable<IReplyMessage>? _messageEdited;
    private IObservable<Message>? _messageSentSub;

    public IObservable<Message> Stanzas => _stanzas ??= _client.Connection.Stanzas.OfType<Message>();

    public IObservable<IMessageEvent> Events => _events ??= _messageEvents.AsObservable();

    public IObservable<IComposing> Composing => _composing ??= Events.OfType<IComposing>();

    public IObservable<IJoinAnnouncement> JoinAnnouncements => _joinAnnouncements ??= Events.OfType<IJoinAnnouncement>();

    public IObservable<IReplyMessage> All => _all ??= Events
        .OfType<RoomMessage>()
        .Select(Contextualize)
        .Where(t => t is not null)
        .Select(t => t!);

    public IObservable<IHubReplyMessage> FromHub => _fromHub ??= All
        .Where(t => t.Context == ContextType.Hub)
        .Cast<IHubReplyMessage>();

    public IObservable<IMatchReplyMessage> FromMatch => _fromMatch ??= All
        .Where(t => t.Context == ContextType.Match)
        .Cast<IMatchReplyMessage>();

    public IObservable<ITeamReplyMessage> FromTeam => _fromTeam ??= All
        .Where(t => t.Context == ContextType.Team)
        .Cast<ITeamReplyMessage>();

    public IObservable<IMessageDeleted> MessageDeleted => _messageDeleted ??= Events.OfType<IMessageDeleted>();

    public IObservable<IReplyMessage> MessageEdited => _messageEdited ??= All.Where(t => t.Edited is not null);

    public IObservable<Message> MessageSent => _messageSentSub ??= _messageSent.AsObservable();
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

    public async Task<bool> Mute(string userId, string hubId, TimeSpan duration)
    {
        var mute = SendMute.HubMute(userId, hubId, duration, _resourceId.Next());
        var result = (Iq)await _client.Connection.Send(mute);
        if (result is not null && !result.HasError) return true;

        if (result is null)
        {
            Error("Mute failed: No response from server: User: {userId} >> Hub: {hubId}", userId, hubId);
            return false;
        }

        Error("Mute failed: {userId} >> Hub: {hubId} >> {data}", userId, hubId, string.Join("\r\n", result.Errors.Select(t => t.Value)));
        return false;
    }

    public async Task<bool> Unmute(string userId, string hubId)
    {
        var unmute = SendMute.HubUnmute(userId, hubId, _resourceId.Next());
        var result = (Iq)await _client.Connection.Send(unmute);
        if (result is not null && !result.HasError) return true;

        if (result is null)
        {
            Error("Unmute failed: No response from server: {userId} >> Hub: {hubId}", userId, hubId);
            return false;
        }

        Error("Unmute failed: {userId} >> Hub: {hubId} >> {data}", userId, hubId, string.Join("\r\n", result.Errors.Select(t => t.Value)));
        return false;
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

        var result = (Message)await _client.Connection.Send(stanza);
        _messageSent.OnNext(result);
        return result;
    }

    public async Task<Iq> Edit(JID to, string messageId, string content)
    {
        var edit = new SendEdit
        {
            To = to.GetBareJID(),
            MessageId = messageId,
            Content = content,
            ResourceId = _resourceId.Next().ToString()
        };
        return (Iq)await _client.Connection.Send(edit);
    }

    public Task<Iq> Edit(IRoomMessage message, string content)
    {
        if (string.IsNullOrEmpty(message.MessageId))
            throw new ArgumentException("Message does not have a resource id");

        var to = message.From.GetBareJID();
        if (to == _client.Auth.Jid.GetBareJID())
            to = message.To.GetBareJID();

        return Edit(to, message.MessageId, content);
    }

    public Task<Iq> Edit(Message message, string content)
    {
        var mid = message.Archiveds.FirstOrDefault()?.Id;
        if (string.IsNullOrEmpty(mid))
            throw new ArgumentException("Message does not have a resource id");

        if (message.From is null)
            throw new ArgumentException("Message does not have a from jid");

        var to = message.From.GetBareJID();
        if (to == _client.Auth.Jid.GetBareJID())
            to = message.To?.GetBareJID() ?? throw new ArgumentException("Message does not have a from jid");
        return Edit(to, mid, content);
    }

    public async Task<Iq> Delete(JID to, string messageId)
    {
        var delete = new SendDelete
        {
            To = to.GetBareJID(),
            MessageId = messageId,
            ResourceId = _resourceId.Next().ToString()
        };
        return (Iq)await _client.Connection.Send(delete);
    }

    public Task<Iq> Delete(IRoomMessage message)
    {
        if (string.IsNullOrEmpty(message.MessageId))
            throw new ArgumentException("Message does not have a resource id");

        var to = message.From.GetBareJID();
        if (to == _client.Auth.Jid.GetBareJID())
            to = message.To.GetBareJID();

        return Delete(to, message.MessageId);
    }

    public Task<Iq> Delete(Message message)
    {
        var mid = message.Archiveds.FirstOrDefault()?.Id;
        if (string.IsNullOrEmpty(mid))
            throw new ArgumentException("Message does not have a resource id");

        if (message.To is null)
            throw new ArgumentException("Message does not have a from jid");

        var to = message.To.GetBareJID();
        return Delete(to, mid);
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
            .Where(t => t.IsMention && !t.IsEveryone && !t.IsHere)
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

    public async IAsyncEnumerable<IMessageEvent> ProcessReadEvent(Message stanza)
    {
        var reads = stanza.Children.OfType<Message.Read>();
        foreach(var read in reads)
        {
            if (read.Jid is null) continue;

            var userId = read.Jid.Resource;
            if (string.IsNullOrEmpty(userId)) continue;

            FaceitUser? profile;
            if (userId == _client.Auth.Id)
                profile = _client.Auth.Profile;
            else
                profile = await _client.Cache.User(userId);

            if (profile is null) continue;

            var type = read.Jid.Context(out string? matchId, out string? teamId, out string? hubId, out _);
            var context = new ParsedContext(type, matchId, teamId, hubId, userId);
            var timestamp = read.Timestamp ?? stanza.Timestamp ?? stanza.Datas.FirstOrDefault()?.Timestamp ?? DateTime.UtcNow;
            var (hub, match, team, _) = await GetData(context);
            var left = match?.Teams.Where(t => t.Key == Config.Chat.FactionLeft && t.Value.Id == team?.Id).Any() ?? true;

            yield return new ReadReceipt 
            {
                Context = type,
                Author = profile,
                Timestamp = timestamp,
                From = stanza.From!,
                To = stanza.To!,
                ResourceId = stanza.Id,
                Team = team!,
                Hub = hub!,
                Match = match!,
            };
        }
    }

    public (IMessageEvent[], bool) ProcessComposing(ResolvedContext context, Message stanza)
    {
        var (hub, match, team, left, user, type, timestamp, from, to) = context;

        var composing = stanza.Composings.FirstOrDefault();
        if (composing is null) return ([], false);
        if (user is null) return ([], true);

        return (
        [ 
            new Composing
            {
                Context = type,
                Author = user,
                Timestamp = timestamp,
                From = from,
                To = to,
                ResourceId = stanza.Id,
                Team = team!,
                Hub = hub!,
                Match = match!,
            }
        ], true);
    }

    public async IAsyncEnumerable<IMessageEvent> ProcessJoins(ResolvedContext context, Message stanza)
    {
        var (hub, match, team, left, user, type, timestamp, from, to) = context;

        var joins = stanza.UserJoins.ToArray();
        if (joins.Length <= 0) yield break;


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
                    From = from,
                    To = to,
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
    }

    public async IAsyncEnumerable<IMessageEvent> ProcessMessages(ResolvedContext context, Message stanza)
    {
        var (hub, match, team, left, user, type, timestamp, from, to) = context;
        var mentions = await ResolveMentions(stanza.Mentions);
        var everyone = stanza.Mentions.Any(t => t.IsEveryone);
        var here = stanza.Mentions.Any(t => t.IsHere);

        var messageId = stanza.Archiveds.FirstOrDefault()?.Id;

        var edit = stanza.Extras
            .FirstOrDefault(t => t.IsEditing)
            ?.Timestamp;

        yield return new RoomMessage
        {
            MessageId = messageId,
            Edited = edit,
            Author = user!,
            Timestamp = timestamp,
            From = from,
            To = to,
            Context = type,
            ResourceId = stanza.Id,
            LeftSide = left,
            Content = stanza.StrBody ?? string.Empty,
            Mentions = mentions,
            MentionsEveryone = everyone,
            MentionsHere = here,
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

    public IEnumerable<IMessageEvent> ProcessDeletes(ResolvedContext context, Message stanza)
    {
        var (hub, match, team, _, user, type, timestamp, from, to) = context;
        foreach (var delete in stanza.DeletedMessages)
        {
            yield return new MessageDeleted 
            {
                Author = user!,
                Timestamp = timestamp,
                From = from,
                To = to,
                Context = type,
                Team = team!,
                Hub = hub!,
                Match = match!,
                ResourceId = stanza.Id,
                MessageId = delete,
            };
        }
    }

    public async Task<FaceitUser?> FindBackupUser(Message stanza)
    {
        string? FromEdit()
        {
            var editor = stanza.Extras.FirstOrDefault(t => t.IsEditing);
            if (editor is null || string.IsNullOrEmpty(editor.By)) return null;

            var by = new JID(editor.By).Node;
            if (string.IsNullOrEmpty(by)) return null;

            return by;
        }

        string? FromArchived()
        {
            var archived = stanza.Archiveds.FirstOrDefault();
            if (archived is null || string.IsNullOrEmpty(archived.By?.Node)) return null;

            return archived.By.Node;
        }

        var by = FromEdit() ?? FromArchived();
        if (string.IsNullOrEmpty(by)) return null;
        return await _client.Cache.User(by);
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
            if (stanza.From.GetBareJID() == stanza.To.GetBareJID())
            {
                await foreach(var evt in ProcessReadEvent(stanza))
                    yield return evt;
                yield break;
            }

            Warning("MESSAGE MODULE >> PARSE MESSAGE >> Unknown context: {event}", stanza.Element.ToXmlString());
            yield break;
        }

        var context = new ParsedContext(type, matchId, teamId, hubId, userId);
        var timestamp = stanza.Timestamp ?? stanza.Datas.FirstOrDefault()?.Timestamp ?? DateTime.UtcNow;
        var (hub, match, team, user) = await GetData(context);
        var left = match?.Teams.Where(t => t.Key == Config.Chat.FactionLeft && t.Value.Id == team?.Id).Any() ?? true;

        var resolved = new ResolvedContext(hub, match, team, left, user, type, timestamp, stanza.From, stanza.To);

        var (events, handled) = ProcessComposing(resolved, stanza);
        if (handled)
        {
            foreach(var evt in events)
                yield return evt;
            yield break;
        }

        await foreach(var evt in ProcessJoins(resolved, stanza))
        {
            handled = true;
            yield return evt;
        }

        if (handled) yield break;

        foreach (var evt in ProcessDeletes(resolved, stanza))
        {
            handled = true;
            yield return evt;
        }

        if (handled) yield break;

        if (user is null)
        {
            user = await FindBackupUser(stanza);
            if (user is null) yield break;

            resolved = new ResolvedContext(hub, match, team, left, user, type, timestamp, stanza.From, stanza.To);
        }

        await foreach(var evt in ProcessMessages(resolved, stanza))
            yield return evt;
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
        //Only call the `ParseMessage` method once per stanza
        Manage(Stanzas
            .SelectMany(t => ParseMessage(t).ToObservable())
            .Where(t => t.Author?.UserId != _client.Auth.Id)
            .Where(t => !MessageHandler(t))
            .Subscribe(_messageEvents.OnNext));
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
        _messageEvents.Dispose();
        _stanzas = null;
        _events = null;
        _composing = null;
        _joinAnnouncements = null;
        _all = null;
        _fromHub = null;
        _fromMatch = null;
        _fromTeam = null;
        return base.OnCleanup();
    }
}

internal record class ParsedContext(
    ContextType Type,
    string? MatchId,
    string? TeamId,
    string? HubId,
    string? UserId);

internal record class ResolvedContext(
    FaceitHub? Hub,
    FaceitMatch? Match,
    FaceitTeam? Team,
    bool LeftSide,
    FaceitUser? User,
    ContextType Type,
    DateTime Timestamp,
    JID From,
    JID To);