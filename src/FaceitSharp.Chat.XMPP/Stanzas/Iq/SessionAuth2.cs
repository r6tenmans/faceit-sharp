namespace FaceitSharp.Chat.XMPP.Stanzas;

public class SessionAuth2 : IStanzaRequest
{
    public const string ID = "_session_auth_2";

    public void Expects(IResponseExpected response)
    {
        response
            .Where<Iq>(t => t.Id == ID && t.Type == IqType.Result);
    }

    public XmlElement Serialize()
    {
        var session = X.C("session", Namespaces.SESSION, []);
        return X.C("iq", Namespaces.CLIENT, [("type", "set"), ("id", ID)], session);
    }

    public static SessionAuth2 Create() => new();
}
