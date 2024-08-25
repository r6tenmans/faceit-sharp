namespace FaceitSharp.Chat.Messaging;

public interface IMessageSubscriptions<TMessage, TComposing, TJoinAnnouncement> : IDisposable
    where TMessage: IRoomMessage
    where TComposing: IComposing
    where TJoinAnnouncement: IJoinAnnouncement
{
    IObservable<TMessage> Message { get; }

    IObservable<TComposing> Composing { get; }

    IObservable<TJoinAnnouncement> JoinAnnouncement { get; }
}

internal class MessageSubscriptions<TMessage, TComposing, TJoinAnnouncement> 
    : IMessageSubscriptions<TMessage, TComposing, TJoinAnnouncement>
    where TMessage : IRoomMessage
    where TComposing : IComposing
    where TJoinAnnouncement : IJoinAnnouncement
{
    private readonly Subject<TMessage> _message = new();
    private readonly Subject<TComposing> _composing = new();
    private readonly Subject<TJoinAnnouncement> _joinAnnouncement = new();
    
    public IObservable<TMessage> Message => _message;

    public IObservable<TComposing> Composing => _composing;

    public IObservable<TJoinAnnouncement> JoinAnnouncement => _joinAnnouncement;

    public void Broadcast(TMessage message) => _message.OnNext(message);

    public void Broadcast(TComposing composing) => _composing.OnNext(composing);

    public void Broadcast(TJoinAnnouncement joinAnnouncement) => _joinAnnouncement.OnNext(joinAnnouncement);

    public void Dispose()
    {
        _message.Dispose();
        _composing.Dispose();
        _joinAnnouncement.Dispose();
    }
}

/// <summary>
/// Subscriptions related to hubs
/// </summary>
public interface IHubSubscriptions : IMessageSubscriptions<IHubMessage, IHubComposing, IHubAnnouncement> { }

internal class HubSubscriptions : MessageSubscriptions<IHubMessage, IHubComposing, IHubAnnouncement>, IHubSubscriptions { }

/// <summary>
/// Subscriptions related to matches
/// </summary>
public interface IMatchSubscriptions : IMessageSubscriptions<IMatchMessage, IMatchComposing, IMatchAnnouncement> { }

internal class MatchSubscriptions : MessageSubscriptions<IMatchMessage, IMatchComposing, IMatchAnnouncement>, IMatchSubscriptions { }

/// <summary>
/// Subscriptions related to teams
/// </summary>
public interface ITeamSubscriptions : IMessageSubscriptions<ITeamMessage, ITeamComposing, ITeamAnnouncement> { }

internal class TeamSubscriptions : MessageSubscriptions<ITeamMessage, ITeamComposing, ITeamAnnouncement>, ITeamSubscriptions { }