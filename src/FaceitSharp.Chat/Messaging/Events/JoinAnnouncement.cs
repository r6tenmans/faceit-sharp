namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Indicates that a user has joined a chat room
/// </summary>
public interface IJoinAnnouncement : IMessageEvent { }

/// <summary>
/// Indicates that a user has joined a hub chat
/// </summary>
public interface IHubAnnouncement : IHubEvent, IJoinAnnouncement { }

/// <summary>
/// Indicates that a user has joined a team chat
/// </summary>
public interface ITeamAnnouncement : ITeamEvent, IJoinAnnouncement { }

/// <summary>
/// Indicates that a user has joined a match room
/// </summary>
public interface IMatchAnnouncement : IMatchEvent, IJoinAnnouncement { }

internal class JoinAnnouncement : MessageEvent, IHubAnnouncement, ITeamAnnouncement, IMatchAnnouncement { }
