namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Auth : IStanzaRequest
{
    public string Mechanism { get; set; } = "PLAIN";

    public required string Value { get; set; }

    public XmlElement Serialize()
    {
        return X.C("auth", Namespaces.SASL, [("mechanism", Mechanism)], Value);
    }

    public void Expects(IResponseExpected response)
    {
        response
            .ShouldBe<Success>()
            .ShouldBe<Failure>()
            .ShouldBe<Challenge>();
    }

    public static Auth Login(string userId, string token, string host = "faceit.com")
    {
        var crafted = $"{userId}@{host}\0{userId}\0{token}";
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(crafted));
        return new Auth { Value = encoded };
    }

    public class Failure(XmlElement element) : Stanza(element)
    {
        public override string? DefaultNamespace() => Namespaces.SASL;
    }

    public class Success(XmlElement element) : Stanza(element)
    {
        public override string? DefaultNamespace() => Namespaces.SASL;
    }

    public class Challenge(XmlElement element) : Stanza(element)
    {
        public string Value { get; } = element.InnerText;

        public override string? DefaultNamespace() => Namespaces.SASL;
    }
}
