namespace FaceitSharp.Chat;

using Messaging;

public interface IFaceitChatSubscriptions
{
    public IHubSubscriptions Hubs { get; }

    public ITeamSubscriptions Teams { get; }

    public IMatchSubscriptions Matches { get; }

    Task<IChatRoomMatch> GetMatch(string id);

    Task<IChatRoomHub> GetHub(string id);

    Task<Message> SendGroupMessage(JID to, string message, params UserMention[] mentions);

    Task<Message> SendGroupMessage(JID to, string message, string[] images, params UserMention[] mentions);
}

internal partial class FaceitChat
{
    private readonly HubSubscriptions _hubs = new();
    private readonly TeamSubscriptions _teams = new();
    private readonly MatchSubscriptions _matches = new();
    
    public IHubSubscriptions Hubs => _hubs;
    public ITeamSubscriptions Teams => _teams;
    public IMatchSubscriptions Matches => _matches;

    private readonly ConcurrentDictionary<string, IChatRoomMatch> _roomMatchSubscriptions = [];
    private readonly ConcurrentDictionary<string, IChatRoomHub> _roomHubSubscriptions = [];

    public async Task<IChatRoomMatch> GetMatch(string id)
    {
        if (_roomMatchSubscriptions.TryGetValue(id, out var room))
            return room;
        
        var match = await _api.Match(id)
            ?? throw new Exception("Match not found");

        await SubscribeMatchAndTeams(match);

        room = new ChatRoomMatch(this, _chat, _api, match);
        _roomMatchSubscriptions.TryAdd(id, room);
        return room;
    }

    public async Task<IChatRoomHub> GetHub(string id)
    {
        if (_roomHubSubscriptions.TryGetValue(id, out var room))
            return room;

        var hub = await _api.Hub(id)
            ?? throw new Exception("Hub not found");

        await SubscribeHub(hub);
        room = new ChatRoomHub(this, _chat, _api, hub);
        _roomHubSubscriptions.TryAdd(id, room);
        return room;
    }

    public static IEnumerable<MessageMention> From(string body, UserMention[] mentions)
    {
        foreach (var mention in mentions)
        {
            var textToFind = mention.Mention;
            var resourceId = mention.ResourceId;

            int startIndex = 0;
            int index;
            while ((index = body.IndexOf(textToFind, startIndex)) != -1)
            {
                var end = index + textToFind.Length;
                yield return new MessageMention(resourceId, index, end);
                startIndex = end;
            }
        }
    }

    public Task<Message> SendGroupMessage(JID to, string message, params UserMention[] mentions)
        => SendGroupMessage(to, message, [], mentions);

    public async Task<Message> SendGroupMessage(JID to, string message, string[] images, params UserMention[] mentions)
    {
        if (!Ready) throw new Exception("Chat is not ready");

        var stanza = new SendChat
        {
            To = to.GetBareJID(),
            From = Jid!,
            Body = message,
            Id = _resourceId.Next().ToString(),
            ImageIds = images,
            Mentions = From(message, mentions).ToArray()
        };

        return (Message)await _chat.Send(stanza);
    }

    public async Task SubscribeHub(FaceitHub hub)
    {
        var sub = ChatSubscription.Create(SubscriptionType.Hub, hub.Guid, Current!.UserId, Jid!, false, false, 10);
        _ = await _chat.Send(sub) 
            ?? throw new Exception("Failed to subscribe to hub presence: " + hub.Guid);
    }

    public Task SubscribeMatchAndTeams(FaceitMatch match)
    {
        async Task SubscribeMatch(string id)
        {
            var sub = ChatSubscription.Create(SubscriptionType.Match, id, Current!.UserId, Jid!, false, false, 10);
            _ = await _chat.Send(sub) 
                ?? throw new Exception("Failed to subscribe to match presence: " + id);
        }

        async Task SubscribeTeam(FaceitMatch.FaceitTeam team)
        {
            var id = $"{match.Id}_{team.Id}";
            var sub = ChatSubscription.Create(SubscriptionType.Team, id, Current!.UserId, Jid!, false, false, 10);
            _ = await _chat.Send(sub)
                ?? throw new Exception("Failed to subscribe to team presence: " + id);
        }

        var tasks = new List<Task>
        {
            SubscribeMatch(match.Id)
        };

        foreach (var team in match.Teams)
            tasks.Add(SubscribeTeam(team.Value));

        return Task.WhenAll(tasks);
    }

    public void SubscriptionsCleanup()
    {
        _roomMatchSubscriptions.Clear();
        _roomHubSubscriptions.Clear();
        _hubs.Dispose();
        _teams.Dispose();
        _matches.Dispose();
    }

    public void SubscriptionsSetup()
    {
        _disposables.Add(_chat
            .StanzaReceived
            .OfType<Message>()
            .Subscribe(async t =>
            {
                try
                {
                    await HandleMessage(t);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to handle message >> {data}", t.Element.ToXmlString());
                }
            }));
    }

    public static bool TryParseNode(JID jid, out string? matchId, out string? teamId, out string? hubId, out string? userId)
    {
        matchId = null;
        teamId = null;
        hubId = null;
        userId = jid.Resource;

        if (string.IsNullOrEmpty(jid.Node)) return false;

        string id;
        if (jid.Node.StartsWith("team-", StringComparison.InvariantCultureIgnoreCase))
        {
            id = jid.Node[5..];
            if (!id.Contains('_')) return false;

            var parts = id.Split('_');
            matchId = parts.First();
            teamId = parts.Last();
            return true;
        }

        if (jid.Node.StartsWith("hub-", StringComparison.InvariantCultureIgnoreCase))
        {
            hubId = jid.Node[4..].Replace("-general", "");
            return true;
        }

        if (jid.Node.StartsWith("match-", StringComparison.InvariantCultureIgnoreCase))
        {
            matchId = jid.Node[6..];
            return true;
        }

        return false;
    }

    public async Task HandleMessage(Message stanza)
    {
        static Task<T?> NullFetch<T>(string? id, Func<string, Task<T?>> func)
        {
            if (string.IsNullOrEmpty(id)) return Task.FromResult<T?>(default);
            return func(id);
        }

        //Ensure we have a "from"
        if (stanza.From is null || stanza.From.Node is null || stanza.To is null || stanza.From == Jid) return;
        //Get the type of node and it's IDs
        if (!TryParseNode(stanza.From, 
            out var matchId, 
            out var teamId, 
            out var hubId, 
            out var userId))
            return;
        //Get the timestamp
        var timestamp = stanza.Timestamp ?? stanza.Datas.FirstOrDefault()?.Timestamp ?? DateTime.UtcNow;
        //Fetch all of the data
        var matchTask = NullFetch(matchId, t => _api.Match(t));
        var hubTask = NullFetch(hubId, t => _api.Hub(t));
        var userTask = NullFetch(userId, t => _api.User(t));

        await Task.WhenAll(matchTask, hubTask, userTask);

        var match = await matchTask;
        var hub = await hubTask;
        var team = match?.Teams.FirstOrDefault(t => t.Value.Id == teamId);
        var right = team?.Key != "faction1";
        var user = await userTask;

        //Check if it's a message composing event
        var composing = stanza.Composings.FirstOrDefault();
        if (composing is not null)
        {
            if (user is null) return;

            var composer = new RoomComposing 
            { 
                Author = user, 
                Timestamp = timestamp, 
                From = stanza.From,
                To = stanza.To,
                ResourceId = stanza.Id
            };

            if (team is not null && team.HasValue) composer.Team = team.Value.Value;
            if (hub is not null) composer.Hub = hub;
            if (match is not null) composer.Match = match;

            if (!string.IsNullOrEmpty(teamId)) _teams.Broadcast(composer);
            else if (!string.IsNullOrEmpty(hubId)) _hubs.Broadcast(composer);
            else if (!string.IsNullOrEmpty(matchId))  _matches.Broadcast(composer);

            return;
        }

        //Check if it's a join event
        var joins = stanza.UserJoins.ToArray();
        if (joins.Length > 0)
        {
            await Task.WhenAll(joins.Select(async j =>
            {
                if (string.IsNullOrEmpty(j.Node)) return;

                var user = await _api.User(j.Node);
                if (user is null) return;

                var join = new JoinAnnouncement
                {
                    Author = user,
                    Timestamp = timestamp,
                    From = stanza.From,
                    To = stanza.To,
                    ResourceId = stanza.Id
                };

                if (team is not null && team.HasValue) join.Team = team.Value.Value;
                if (hub is not null) join.Hub = hub;
                if (match is not null) join.Match = match;

                if (!string.IsNullOrEmpty(teamId)) _teams.Broadcast(join);
                else if (!string.IsNullOrEmpty(hubId)) _hubs.Broadcast(join);
                else if (!string.IsNullOrEmpty(matchId)) _matches.Broadcast(join);
            }));
            return;
        }

        if (user is null) return;

        var mentions = stanza.Mentions.ToArray();
        var everyone = mentions.Any(t => t.IsEveryone);

        var mentioned = (await Task.WhenAll(mentions
            .Where(t => t.IsMention && !t.IsEveryone)
            .Select(async m =>
            {
                var uid = m.Uri?.Replace("xmpp:", "").Replace("@faceit.com", "");
                if (string.IsNullOrEmpty(uid)) return null;

                var user = await _api.User(uid);
                if (user is null) return null;

                return user;
            })))
            .Where(t => t is not null)
            .Select(t => t!)
            .ToArray();

        var messageType = MessageType.Hub;
        if (!string.IsNullOrEmpty(teamId)) messageType = MessageType.Team;
        else if (!string.IsNullOrEmpty(hubId)) messageType = MessageType.Hub;
        else if (!string.IsNullOrEmpty(matchId)) messageType = MessageType.Match;

        var message = new RoomMessage
        {
            Author = user,
            Timestamp = timestamp,
            From = stanza.From,
            To = stanza.To,
            MessageType = messageType,
            ResourceId = stanza.Id,
            LeftSide = !right,
            Content = stanza.StrBody ?? string.Empty,
            Mentions = mentioned,
            MentionsEveryone = everyone,
            MentionsCurrentUser = mentioned.Any(t => t.UserId == UserId),
            Type = stanza.Type ?? string.Empty,
            AttachedImages = stanza
                .Images
                .Select(t => t.Src)
                .Where(t => !string.IsNullOrEmpty(t))
                .Select(t => t!)
                .ToArray()
        };

        if (team is not null && team.HasValue) message.Team = team.Value.Value;
        if (hub is not null) message.Hub = hub;
        if (match is not null) message.Match = match;

        if (!string.IsNullOrEmpty(teamId)) _teams.Broadcast(message);
        else if (!string.IsNullOrEmpty(hubId)) _hubs.Broadcast(message);
        else if (!string.IsNullOrEmpty(matchId)) _matches.Broadcast(message);
    }
}
