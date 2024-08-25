namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Indicates that a user has joined a chat room
/// </summary>
public interface IJoinAnnouncement : IRoom { }

/// <summary>
/// Indicates that a user has joined a hub chat
/// </summary>
public interface IHubAnnouncement : IRoomHub, IJoinAnnouncement { }

/// <summary>
/// Indicates that a user has joined a team chat
/// </summary>
public interface ITeamAnnouncement : IRoomTeam, IJoinAnnouncement { }

/// <summary>
/// Indicates that a user has joined a match room
/// </summary>
public interface IMatchAnnouncement : IRoomMatch, IJoinAnnouncement { }

internal class JoinAnnouncement : Room, IHubAnnouncement, ITeamAnnouncement, IMatchAnnouncement { }
