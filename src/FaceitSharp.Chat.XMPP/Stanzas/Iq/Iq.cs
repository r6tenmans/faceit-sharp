namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Iq : Stanza
{
    public List<IIqChild> Children { get; } = [];

    public List<XmlElement> Unknown { get; } = [];

    public IEnumerable<Bind> Binds => Children.OfType<Bind>();

    public IEnumerable<Session> Sessions => Children.OfType<Session>();

    /// <summary>
    /// The type of the IQ stanza.
    /// </summary>
    public IqType? Type
    {
        get => Element.GetAttr<IqType>("type");
        set => Element.SetAttr("type", value);
    }

    /// <summary>
    /// Determines whether the IQ stanza is a request.
    /// </summary>
    public bool IsRequest => Type == IqType.Set || Type == IqType.Get;

    /// <summary>
    /// Determines whether the IQ stanza is a response.
    /// </summary>
    public bool IsResponse => !IsRequest;

    /// <summary>
    /// Initializes a new instance of the Iq class.
    /// </summary>
    /// <param name="data">The content of the stanza.</param>
    public Iq(params XmlElement?[] data) : base(data) { }

    /// <summary>
    /// Initializes a new instance of the Iq class.
    /// </summary>
    /// <param name="element">The XML element</param>
    public Iq(XmlElement element) : base(element)
    {
        foreach (var obj in element.ChildNodes)
        {
            if (obj is not XmlElement child) continue;

            var stanza = Xml.GetStanza(child,
                typeof(Bind), typeof(Session));

            if (stanza is null || stanza is not IIqChild feature)
            {
                Unknown.Add(child);
                continue;
            }

            Children.Add(feature);
        }
    }

    public interface IIqChild { }

    public class Bind : Stanza, IIqChild
    {
        public JID? Jid { get; set; }

        public Bind(XmlElement element) : base(element)
        {
            foreach (var obj in element.ChildNodes)
            {
                if (obj is not XmlElement child) continue;

                if (child.Name == "jid")
                {
                    Jid = new JID(child.InnerText);
                    continue;
                }
            }
        }
    }

    public class Session(XmlElement element) : Stanza(element), IIqChild
    {
        public override string? DefaultNamespace() => Namespaces.SESSION;
    }
}
