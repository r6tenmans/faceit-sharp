namespace FaceitSharp.Chat.XMPP.Stanzas;

public class BindAuth2 : IStanzaRequest
{
    public const string ID = "_bind_auth_2";

    public required string ResourceId { get; set; }

    public void Expects(IResponseExpected response)
    {
        response
            .Where<Iq>(t => t.Id == ID && t.Type == IqType.Result);
    }

    public JID? ProcessResponse(Stanza response)
    {
        if (response is not Iq iq) return null;

        return iq.Binds.FirstOrDefault()?.Jid;
    }

    public XmlElement Serialize()
    {
        var resource = X.C("resource", null, [], ResourceId);
        var bind = X.C("bind", Namespaces.BIND, [], resource);
        return X.C("iq", Namespaces.CLIENT, [("type", "set"), ("id", ID)], bind);
    }

    public static BindAuth2 Create(string resourceId)
    {
        return new BindAuth2 { ResourceId = resourceId };
    }
}
