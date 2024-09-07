
namespace FaceitSharp.Chat.XMPP.Stanzas;

public class SendDelete : IStanzaRequest
{
    public required JID To { get; set; }

    public required string ResourceId { get; set; }

    public required string MessageId { get; set; }

    public bool Group { get; set; } = true;

    public void Expects(IResponseExpected response)
    {
        response
            .Where<Iq>(t => t.Id == ResourceId && (t.Type == IqType.Result || t.Type == IqType.Error));
    }

    public XmlElement Serialize()
    {
        var ns = Group ? Namespaces.CHAT_GROUP_DELETE : Namespaces.CHAT_DELETE;

        var item = X.C("item", null, [("id", MessageId)]);
        var query = X.C("query", ns, [], item);
        return X.C("iq", null, [("type", "set"), ("id", ResourceId), ("to", To.ToString())], query);
    }
}
