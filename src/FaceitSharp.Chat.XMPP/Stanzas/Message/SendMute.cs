namespace FaceitSharp.Chat.XMPP.Stanzas;

public class SendMute : IStanzaRequest
{
    public required bool IsMute { get; set; }

    public required JID UserId { get; set; }

    public required JID HubId { get; set; }

    public required int TimeToLive { get; set; }

    public required long ResourceId { get; set; }

    public void Expects(IResponseExpected response)
    {
        response
            .Where<Iq>(t => t.Id == ResourceId.ToString() && (t.Type == IqType.Result || t.Type == IqType.Error));
    }

    public XmlElement DoMute()
    {
        return X.C("muted", null, [
            ("jid", UserId.ToString()),
            ("ttl", TimeToLive.ToString())
        ]);
    }

    public XmlElement DoUnmute()
    {
        return X.C("unmuted", null, [("jid", UserId.ToString())]);
    }

    public XmlElement Serialize()
    {
        var mute = IsMute ? DoMute() : DoUnmute();
        var query = X.C("query", Namespaces.MUTE_USER, [], mute);
        return X.C("iq", null, [
            ("type", "set"), 
            ("to", HubId.ToString()),
            ("id", ResourceId.ToString())
        ], query);
    }

    public static SendMute Hub(bool isMute, string userId, string hubId, int timeToLive, long resourceId)
    {
        return new()
        {
            IsMute = isMute,
            UserId = new("faceit.com", userId),
            HubId = new("conference.faceit.com", $"hub-{hubId}-general"),
            TimeToLive = timeToLive,
            ResourceId = resourceId
        };
    }

    public static SendMute HubMute(string userId, string hubId, TimeSpan timeToLive, long resourceId)
    {
        return Hub(true, userId, hubId, (int)timeToLive.TotalSeconds, resourceId);
    }

    public static SendMute HubUnmute(string userId, string hubId, long resourceId)
    {
        return Hub(false, userId, hubId, 0, resourceId);
    }
}
