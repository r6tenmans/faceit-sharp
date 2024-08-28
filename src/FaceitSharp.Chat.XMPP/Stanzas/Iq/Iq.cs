namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Iq : Stanza
{
    public List<IIqChild> Children { get; } = [];

    public List<XmlElement> Unknown { get; } = [];

    public IEnumerable<Bind> Binds => Children.OfType<Bind>();

    public IEnumerable<Session> Sessions => Children.OfType<Session>();

    public IEnumerable<Error> Errors => Children.OfType<Error>();

    public bool HasError => Errors.Any();

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

    public static void Parse(XmlElement element, List<IIqChild> children, List<XmlElement> unknown)
    {
        foreach (var obj in element.ChildNodes)
        {
            if (obj is not XmlElement child) continue;

            var stanza = Xml.GetStanza(child,
                typeof(Bind), typeof(Session), typeof(Error), typeof(Text));

            if (stanza is null || stanza is not IIqChild el)
            {
                unknown.Add(child);
                continue;
            }

            children.Add(el);
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

    public class Text(XmlElement element) : Stanza(element), IIqChild
    {
        public override string? DefaultNamespace()
        {
            return Namespaces.STANZAS;
        }

        public string? Value
        {
            get => Element.InnerText?.ForceNull();
            set => Element.InnerText = value ?? string.Empty;
        }
    }

    public class Error : Stanza
    {
        public string? Type
        {
            get => Element.GetAttr("type");
            set => Element.SetAttr("type", value);
        }

        public int? Code
        {
            get => Element.GetAttrInt("code");
            set => Element.SetAttr("code", value);
        }

        public string Value => string.Join("\r\n", Children.OfType<Text>().Select(t => t.Value).Where(t => t != null));

        public List<IIqChild> Children { get; } = [];

        public List<XmlElement> Unknown { get; } = [];

        public Error(XmlElement element) : base(element)
        {
            Parse(element, Children, Unknown);
        }
    }
}
