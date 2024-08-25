namespace FaceitSharp.Chat.Messaging;

using Network;

using Team = FaceitMatch.FaceitTeam;

/// <summary>
/// Represents a match chat that has been subscribed to
/// </summary>
public interface IChatRoomMatch : IMessageSender
{
    /// <summary>
    /// The match that was subscribed to
    /// </summary>
    FaceitMatch Match { get; }

    /// <summary>
    /// The left side team (faction1)
    /// </summary>
    Team LeftSide { get; }

    /// <summary>
    /// The right side team (faction2)
    /// </summary>
    Team RightSide { get; }

    /// <summary>
    /// Triggered whenever a stanza is received that targets the match
    /// </summary>
    IObservable<Stanza> MatchStanzas { get; }

    /// <summary>
    /// Triggered whenever a stanza is received that targets either team
    /// </summary>
    IObservable<(Team, Stanza)> TeamStanzas { get; }

    /// <summary>
    /// Triggered whenever a message is received that targets the match
    /// </summary>
    IObservable<IMatchReplyMessage> MatchMessages { get; }

    /// <summary>
    /// Triggered whenever a message is received that targets the left side (faction1) team
    /// </summary>
    IObservable<ITeamReplyMessage> LeftTeamMessages { get; }

    /// <summary>
    /// Triggered whenever a message is received that targets the right side (faction2) team
    /// </summary>
    IObservable<ITeamReplyMessage> RightTeamMessages { get; }

    /// <summary>
    /// Triggered whenever a message is received that targets either team
    /// </summary>
    IObservable<ITeamReplyMessage> TeamMessages { get; }

    /// <summary>
    /// Triggered whenever a message is received that targets the match or either team
    /// </summary>
    /// <remarks>Items will either be a <see cref="ITeamReplyMessage"/> or <see cref="IMatchReplyMessage"/></remarks>
    IObservable<IReplyMessage> Messages { get; }

    /// <summary>
    /// Sends a message to the team chat
    /// </summary>
    /// <param name="left">Whether to send it to the left side (faction1) chat or right side (faction2) chat</param>
    /// <param name="message">The message</param>
    /// <param name="mentions">Any users to mention</param>
    /// <returns>The result of the message send</returns>
    Task<Message> SendTeam(bool left, string message, params UserMention[] mentions);

    /// <summary>
    /// Sends a message to the team chat
    /// </summary>
    /// <param name="left">Whether to send it to the left side (faction1) chat or right side (faction2) chat</param>
    /// <param name="message">The message</param>
    /// <param name="images">Any images to send</param>
    /// <param name="mentions">Any users to mention</param>
    /// <returns>The result of the message send</returns>
    Task<Message> SendTeam(bool left, string message, string[] images, params UserMention[] mentions);

    /// <summary>
    /// Refreshes the match and team date from FaceIT
    /// </summary>
    Task Refresh();
}

internal class ChatRoomMatch(
    IFaceitChat _chat,
    IChatSocket _socket,
    IFaceitChatCacheService _api,
    FaceitMatch _match) : IChatRoomMatch
{
    #region Data
    private JID? _leftTeamJid;
    private JID? _rightTeamJid;
    private JID? _matchJid;
    private Team? _left;
    private Team? _right;

    public JID MatchJid
    {
        get => _matchJid ??= new("conference.faceit.com", "match-" + Match.Id);
    }
    public JID LeftJid
    {
        get => (_leftTeamJid ??= TeamJid(LeftSide)) ?? throw new Exception("Left Side Team not set");
    }
    public JID RightJid
    {
        get => (_rightTeamJid ??= TeamJid(RightSide)) ?? throw new Exception("Right Side Team not set");
    }

    public FaceitMatch Match { get; private set; } = _match;
    public Team LeftSide
    {
        get => _left ??= Match.Teams.TryGetValue("faction1", out var team) ? team : throw new Exception("Faction 1 not found");
    }
    public Team RightSide
    {
        get => _right ??= Match.Teams.TryGetValue("faction2", out var team) ? team : throw new Exception("Faction 2 not found");
    }
    #endregion

    #region Events
    public IObservable<Stanza> MatchStanzas => _socket.StanzaReceived
        .Where(x => x.To?.GetBareJID() == MatchJid.GetBareJID());

    public IObservable<(Team, Stanza)> TeamStanzas => _socket.StanzaReceived
        .Where(x => x.To?.GetBareJID() == LeftJid.GetBareJID() ||
                    x.To?.GetBareJID() == RightJid.GetBareJID())
        .Select(x => (x.To!.GetBareJID() == LeftJid.GetBareJID() ? LeftSide : RightSide, x));

    public IObservable<IMatchReplyMessage> MatchMessages => _chat.Matches
        .Message.Where(t => t.Match.Id == Match.Id)
        .Select(t => new ReplyMessage(MatchJid, (RoomMessage)t, _chat));

    public IObservable<ITeamReplyMessage> LeftTeamMessages => _chat.Teams
        .Message.Where(t => t.Match.Id == Match.Id && t.Team.Id == LeftSide.Id)
        .Select(t => new ReplyMessage(LeftJid, (RoomMessage)t, _chat));

    public IObservable<ITeamReplyMessage> RightTeamMessages => _chat.Teams
        .Message.Where(t => t.Match.Id == Match.Id && t.Team.Id == RightSide.Id)
        .Select(t => new ReplyMessage(RightJid, (RoomMessage)t, _chat));

    public IObservable<ITeamReplyMessage> TeamMessages => RightTeamMessages.Merge(LeftTeamMessages);

    public IObservable<IReplyMessage> Messages => MatchMessages.Merge(TeamMessages);
    #endregion

    #region Message Senders
    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _chat.SendGroupMessage(MatchJid, message, images, mentions);

    public Task<Message> Send(string message, params UserMention[] mentions)
        => _chat.SendGroupMessage(MatchJid, message, mentions);

    public Task<Message> SendTeam(bool left, string message, string[] images, params UserMention[] mentions)
        => _chat.SendGroupMessage(left ? LeftJid : RightJid, message, images, mentions);

    public Task<Message> SendTeam(bool left, string message, params UserMention[] mentions)
        => _chat.SendGroupMessage(left ? LeftJid : RightJid, message, mentions);
    #endregion

    public JID? TeamJid(Team? team)
    {
        if (team is null) return null;

        var node = $"team-{Match.Id}_{team?.Id}";
        return new("conference.faceit.com", node);
    }

    public async Task Refresh()
    {
        Match = await _api.Match(Match.Id, true) ?? throw new Exception("Match not found");
        _leftTeamJid = null;
        _rightTeamJid = null;
        _matchJid = null;
        _left = null;
        _right = null;
    }
}
