namespace FaceitSharp.Chat;

public interface IFaceitChatSubscriptions
{
    Task<IObservable<Stanza>> Subscribe(SubscriptionType type, string id,
        bool presenceInit = false, bool presenceUpdate = false, int priority = 10);

    Task<IObservable<Stanza>> OnHub(string id);

    Task<IObservable<Stanza>> OnLobby(string id);

    Task<IObservable<Stanza>> OnMatch(string id);

    Task<IObservable<Stanza>> OnTeam(string matchId, string teamId);
}

internal partial class FaceitChat
{
    private readonly Dictionary<string, Subject<Stanza>> _subscriptions = [];

    public async Task<IObservable<Stanza>> Subscribe(SubscriptionType type, string id, 
        bool presenceInit = false, bool presenceUpdate = false, int priority = 10)
    {
        if (!Ready) throw new InvalidOperationException("Not ready");

        var subscription = ChatSubscription.Create(type, id, Current!.UserId, Jid!, presenceInit, presenceUpdate, priority);
        var target = subscription.To.GetBareJID().ToString();

        if (_subscriptions.TryGetValue(target, out var sub))
            return sub.AsObservable();

        var subject = new Subject<Stanza>();

        _ = await _chat.Send(subscription) 
            ?? throw new Exception("Failed to subscribe to presence: " + target);

        _subscriptions.Add(target, subject);
        return subject.AsObservable();
    }

    public async Task<IObservable<Stanza>> OnHub(string id)
        => await Subscribe(SubscriptionType.Hub, id);

    public async Task<IObservable<Stanza>> OnLobby(string id)
        => await Subscribe(SubscriptionType.Lobby, id);

    public async Task<IObservable<Stanza>> OnMatch(string id)
        => await Subscribe(SubscriptionType.Match, id);

    public async Task<IObservable<Stanza>> OnTeam(string matchId, string teamId)
        => await Subscribe(SubscriptionType.Team, $"{matchId}_{teamId}");

    public void SubscriptionsCleanup()
    {
        foreach (var sub in _subscriptions.Values)
            sub.Dispose();

        _subscriptions.Clear();
    }

    public void SubscriptionsSetup()
    {
        _chat
            .StanzaReceived
            .Subscribe(t =>
            {
                if (t.From is null || 
                    !_subscriptions.TryGetValue(t.From.GetBareJID().ToString(), out var sub))
                    return;

                sub.OnNext(t);
            });
    }
}
