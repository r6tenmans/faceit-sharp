namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Open : Stanza, IStanzaRequest
{
    public string? Version
    {
        get => Element.GetAttr("version");
        set => Element.SetAttr("version", value);
    }

    internal Open() { }

    public Open(XmlElement element) : base(element) { }

    public override string? DefaultNamespace() => Namespaces.FRAMING;

    public XmlElement Serialize() => Element;

    public void Expects(IResponseExpected response)
    {
        response
            .ShouldBe<Features>()
            .ShouldBe<Open>();
    }

    public static Open Create(string version = "1.0", string to = "faceit.com")
    {
        return new Open
        {
            Version = version,
            To = to,
            Namespace = Namespaces.FRAMING
        };
    }
}
