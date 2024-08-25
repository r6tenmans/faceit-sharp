namespace FaceitSharp.Chat.XMPP.Stanzas;

public class ChatSubscription : IStanzaRequest
{
    public required JID To { get; init; }

    public required JID From { get; init; }

    public required bool InitialPresences { get; set; }

    public required bool PresenceUpdates { get; set; }

    public required int Priority { get; set; }

    public void Expects(IResponseExpected response)
    {
        response
            .Where<Presence>(t => t.From == To && t.To == From);
    }

    public XmlElement Serialize()
    {
        var history = X.C("history", null, [("maxstanzas", "0")]);
        var x = X.C("x", Namespaces.MUC, [], history);

        var unsubs = new List<XmlElement>();

        if (!InitialPresences)
            unsubs.Add(X.C("initial_presences", null, []));
        if (!PresenceUpdates)
            unsubs.Add(X.C("presence_updates", null, []));

        var unsubscribe = X.C("unsubscribe", null, [], unsubs.ToArray());
        var priority = X.C("priority", null, [], Priority.ToString());

        return X.C("presence", Namespaces.CLIENT,
            [("to", To.ToString()), ("from", From.ToString())], 
            x, unsubscribe, priority);
    }

    public static ChatSubscription Create(SubscriptionType type, string id, string userId, JID from, 
        bool presenceInit, bool presenceUpdate, int priority)
    {
        if (type == SubscriptionType.Hub && !id.EndsWith("-general"))
            id += "-general";

        var to = $"{type.ToString().ToLower()}-{id}@conference.faceit.com/{userId}";

        return new()
        {
            To = to!,
            From = from,
            InitialPresences = presenceInit,
            PresenceUpdates = presenceUpdate,
            Priority = priority
        };
    }
}
