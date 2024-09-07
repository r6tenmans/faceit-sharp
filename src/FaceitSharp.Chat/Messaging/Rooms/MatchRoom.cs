namespace FaceitSharp.Chat.Messaging.Rooms;

/// <summary>
/// Represents a match room on FaceIT that can be interacted with
/// </summary>
public interface IMatchRoom : IMessageSender
{
    #region Data
    /// <summary>
    /// The information for the match
    /// </summary>
    FaceitMatch Match { get; }

    /// <summary>
    /// The information for the left side team
    /// </summary>
    FaceitTeam Left { get; }

    /// <summary>
    /// The information for the right side team
    /// </summary>
    FaceitTeam Right { get; }

    /// <summary>
    /// The JIDs of all chats in the match room
    /// </summary>
    JID[] Ids { get; }
    #endregion

    #region Events
    /// <summary>
    /// Messages from the match chat
    /// </summary>
    IObservable<IMatchReplyMessage> MatchChat { get; }

    /// <summary>
    /// Messages from either team chat
    /// </summary>
    IObservable<ITeamReplyMessage> TeamChat { get; }

    /// <summary>
    /// Messages from the left side team chat
    /// </summary>
    IObservable<ITeamReplyMessage> LeftTeamChat { get; }

    /// <summary>
    /// Messages from the right side team chat
    /// </summary>
    IObservable<ITeamReplyMessage> RightTeamChat { get; }

    /// <summary>
    /// Messages from either the match chat or either teams' chats
    /// </summary>
    IObservable<IMatchReplyMessage> Messages { get; }

    /// <summary>
    /// Indicates that a message is being composed in the match or team chats
    /// </summary>
    IObservable<IMatchComposing> Composing { get; }

    /// <summary>
    /// Indicates that a message is being composed in the match chat
    /// </summary>
    IObservable<IMatchComposing> MatchChatComposing { get; }

    /// <summary>
    /// Indicates that a message is being composed in a team chat
    /// </summary>
    IObservable<ITeamComposing> TeamChatComposing { get; }

    /// <summary>
    /// Indicates that a message is being composed in the left side team chat
    /// </summary>
    IObservable<ITeamComposing> LeftTeamChatComposing { get; }

    /// <summary>
    /// Indicates that a message is being composed in the right side team chat
    /// </summary>
    IObservable<ITeamComposing> RightTeamChatComposing { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Refreshes the data for the match
    /// </summary>
    Task Refresh();

    /// <summary>
    /// Sends a message to the team chat
    /// </summary>
    /// <param name="left">Whether to send to the left or right team</param>
    /// <param name="message">The message</param>
    /// <param name="images">Any images to send</param>
    /// <param name="mentions">Any users to mention</param>
    /// <returns>The result of the message send</returns>
    Task<Message> SendTeam(bool left, string message, string[] images, params UserMention[] mentions);

    /// <summary>
    /// Sends a message to the team chat
    /// </summary>
    /// <param name="left">Whether to send to the left or right team</param>
    /// <param name="message">The message</param>
    /// <param name="mentions">Any users to mention</param>
    /// <returns>The result of the message send</returns>
    Task<Message> SendTeam(bool left, string message, params UserMention[] mentions);
    #endregion
}

internal class MatchRoom(
    FaceitMatch _initial,
    IFaceitChatClient _client) : IMatchRoom
{
    #region Data
    private JID? _matchId;
    private JID? _leftId;
    private JID? _rightId;
    private FaceitTeam? _left;
    private FaceitTeam? _right;

    public FaceitMatch Match { get; set; } = _initial;
    public JID MatchId => _matchId ??= Match.GetJID();
    public JID LeftId => _leftId ??= Left.GetJID(Match);
    public JID RightId => _rightId ??= Right.GetJID(Match);
    public JID[] Ids => [LeftId, RightId, MatchId];
    public FaceitTeam Left => _left ??= GetTeam(true);
    public FaceitTeam Right => _right ??= GetTeam(false);
    #endregion

    #region Events
    private IObservable<IMatchReplyMessage>? _matchChat;
    private IObservable<ITeamReplyMessage>? _teamChat;
    private IObservable<IMatchReplyMessage>? _messages;
    private IObservable<ITeamReplyMessage>? _leftTeamChat;
    private IObservable<ITeamReplyMessage>? _rightTeamChat;
    private IObservable<IMatchComposing>? _composing;
    private IObservable<IMatchComposing>? _matchChatComposing;
    private IObservable<ITeamComposing>? _teamChatComposing;
    private IObservable<ITeamComposing>? _leftTeamChatComposing;
    private IObservable<ITeamComposing>? _rightTeamChatComposing;

    public IObservable<IMatchReplyMessage> MatchChat
        => _matchChat ??= _client.Messages.FromMatch.Where(t => t.Match.Id == Match.Id);

    public IObservable<ITeamReplyMessage> TeamChat 
        => _teamChat ??= _client.Messages.FromTeam.Where(t => t.Match.Id == Match.Id);

    public IObservable<IMatchReplyMessage> Messages 
        => _messages ??= MatchChat.Merge(TeamChat);

    public IObservable<ITeamReplyMessage> LeftTeamChat 
        => _leftTeamChat ??= TeamChat.Where(t => t.Team.Id == Left.Id);

    public IObservable<ITeamReplyMessage> RightTeamChat 
        => _rightTeamChat ??= TeamChat.Where(t => t.Team.Id == Right.Id);

    public IObservable<IMatchComposing> Composing 
        => _composing ??= _client.Messages.Composing
            .Where(t => t.Context == ContextType.Match 
                || t.Context == ContextType.Team).Cast<IMatchComposing>()
            .Where(t => t.Match.Id == Match.Id);

    public IObservable<IMatchComposing> MatchChatComposing 
        => _matchChatComposing ??= Composing.Where(t => t.Context == ContextType.Match);

    public IObservable<ITeamComposing> TeamChatComposing 
        => _teamChatComposing ??= Composing.Where(t => t.Context == ContextType.Team).Cast<ITeamComposing>();

    public IObservable<ITeamComposing> LeftTeamChatComposing 
        => _leftTeamChatComposing ??= TeamChatComposing.Where(t => t.Team.Id == Left.Id);

    public IObservable<ITeamComposing> RightTeamChatComposing 
        => _rightTeamChatComposing ??= TeamChatComposing.Where(t => t.Team.Id == Right.Id);
    #endregion

    public void SetContext(FaceitMatch match)
    {
        Match = match;
        var (jid, left, right) = match.GetJIDs();
        _matchId = jid;
        _leftId = left;
        _rightId = right;

        _left = match.Teams.TryGetValue(_client.Config.Chat.FactionLeft, out var team) ? team : null;
        _right = match.Teams.TryGetValue(_client.Config.Chat.FactionRight, out team) ? team : null;
    }

    public async Task Refresh()
    {
        var match = await _client.Cache.Match(Match.Id, true) 
            ?? throw new Exception("Match not found");
        SetContext(match);
    }

    public FaceitTeam GetTeam(bool left)
    {
        var key = left ? _client.Config.Chat.FactionLeft : _client.Config.Chat.FactionRight;
        return Match.Teams.TryGetValue(key, out var team) 
            ? team 
            : throw new InvalidOperationException("Team not found");
    }

    #region Message Senders
    public Task<Message> Send(string message, string[] images, params UserMention[] mentions)
        => _client.Messages.Send(MatchId, message, images, mentions);

    public Task<Message> Send(string message, params UserMention[] mentions)
        => Send(message, [], mentions);

    public Task<Message> SendTeam(bool left, string message, string[] images, params UserMention[] mentions)
        => _client.Messages.Send(left ? LeftId : RightId, message, images, mentions);

    public Task<Message> SendTeam(bool left, string message, params UserMention[] mentions)
        => SendTeam(left, message, [], mentions);
    #endregion
}
