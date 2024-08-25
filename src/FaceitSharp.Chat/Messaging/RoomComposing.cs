namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Indicates a message is being composed in a room
/// </summary>
public interface IComposing : IRoom { }

/// <summary>
/// Indicates a message is being composed in a match room
/// </summary>
public interface IMatchComposing : IRoomMatch, IComposing { }

/// <summary>
/// Indicates a message is being composed in a team chat
/// </summary>
public interface ITeamComposing : IRoomTeam, IComposing { }

/// <summary>
/// Indicates a message is being composed in a hub chat
/// </summary>
public interface IHubComposing : IRoomHub, IComposing { }

internal class RoomComposing : Room, IMatchComposing, ITeamComposing, IHubComposing { }
