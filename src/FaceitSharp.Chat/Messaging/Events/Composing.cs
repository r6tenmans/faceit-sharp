namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Indicates a message is being composed in a room
/// </summary>
public interface IComposing : IMessageEvent { }

/// <summary>
/// Indicates a message is being composed in a match room
/// </summary>
public interface IMatchComposing : IMatchEvent, IComposing { }

/// <summary>
/// Indicates a message is being composed in a team chat
/// </summary>
public interface ITeamComposing : ITeamEvent, IComposing { }

/// <summary>
/// Indicates a message is being composed in a hub chat
/// </summary>
public interface IHubComposing : IHubEvent, IComposing { }

internal class Composing : MessageEvent, IMatchComposing, ITeamComposing, IHubComposing { }
