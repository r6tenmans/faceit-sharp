namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Ping : IStanzaRequest
{
    public required string Id { get; set; }

    public string Host { get; set; } = "faceit.com";

    public void Expects(IResponseExpected response)
    {
        response
            .Where<Iq>(t => t.Id == Id && t.Type == IqType.Result);
    }

    public XmlElement Serialize()
    {
        var ping = X.C("ping", Namespaces.PING, []);
        return X.C("iq", Namespaces.CLIENT, 
            [("type", "get"), ("id", Id), ("to", Host)], ping);
    }

    public static Ping Create(long resourceId)
    {
        return new()
        {
            Id = $"{resourceId}:ping"
        };
    }
}
