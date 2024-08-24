namespace FaceitSharp.Chat.XMPP;

public interface IStanzaRequest
{
    XmlElement Serialize();

    void Expects(IResponseExpected response);
}
