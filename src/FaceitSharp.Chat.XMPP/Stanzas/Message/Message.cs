namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Message : Stanza
{
    public string? MamId
    {
        get => Element.GetAttr("mam_id");
        set => Element.SetAttr("mam_id", value);
    }

    public string? Type
    {
        get => Element.GetAttr("type");
        set => Element.SetAttr("type", value);
    }

    public DateTime? Timestamp
    {
        get => Element.GetAttrDate("stamp");
        set => Element.SetAttr("stamp", value);
    }

    public List<IMessageChild> Children { get; } = [];

    public List<XmlElement> Unknown { get; } = [];

    public IEnumerable<Body> Bodies => Children.OfType<Body>();

    public IEnumerable<Data> Datas => Children.OfType<Data>();

    public IEnumerable<Archived> Archiveds => Children.OfType<Archived>();

    public IEnumerable<Reference> Mentions => Children.OfType<Reference>();

    public IEnumerable<Composing> Composings => Children.OfType<Composing>();

    public IEnumerable<Img> Images => Children.OfType<X>().SelectMany(t => t.Images);

    public IEnumerable<JID> UserJoins => Children.OfType<X>().SelectMany(t => t.UserJoins);

    public string? StrBody => string.Join("\n", Bodies.Select(b => b.Value));

    public Message(XmlElement element) : base(element)
    {
        Parse(element, Children, Unknown);
    }

    public static void Parse(XmlElement element, List<IMessageChild> children, List<XmlElement> unknown)
    {
        foreach (var obj in element.ChildNodes)
        {
            if (obj is not XmlElement child) continue;

            var stanza = Xml.GetStanza(child,
                typeof(Body), typeof(Data),
                typeof(Archived), typeof(Reference),
                typeof(Composing), typeof(Read),
                typeof(X), typeof(Img), typeof(Item));

            if (stanza is null || stanza is not IMessageChild el)
            {
                unknown.Add(child);
                continue;
            }

            children.Add(el);
        }
    }

    public override string? DefaultNamespace() => Namespaces.CLIENT;

    public interface IMessageChild { }

    public class Body(XmlElement element) : Stanza(element), IMessageChild
    {
        public string? Value
        {
            get => Element.InnerText?.ForceNull();
            set => Element.InnerText = value ?? string.Empty;
        }
    }

    public class Data(XmlElement element) : Stanza(element), IMessageChild
    {
        public DateTime? Timestamp
        {
            get => Element.GetAttrDate("timestamp");
            set => Element.SetAttr("timestamp", value);
        }
    }

    public class Archived(XmlElement element) : Stanza(element), IMessageChild
    {
        public JID? By
        {
            get => Element.GetAttrJID("by");
            set => Element.SetAttr("by", value);
        }
    }

    public class Reference(XmlElement element) : Stanza(element), IMessageChild
    {
        public string? Type
        {
            get => Element.GetAttr("type");
            set => Element.SetAttr("type", value);
        }

        public bool IsMention => Type == "mention";

        public bool IsEveryone => Uri == "room:everyone";

        public string? Uri
        {
            get => Element.GetAttr("uri"); 
            set => Element.SetAttr("uri", value);
        }

        public int? Begin
        {
            get => Element.GetAttrInt("begin");
            set => Element.SetAttr("begin", value);
        }

        public int? End
        {
            get => Element.GetAttrInt("begin");
            set => Element.SetAttr("begin", value);
        }

        public override string? DefaultNamespace() => Namespaces.REFERENCE;
    }

    public class Composing(XmlElement element) : Stanza(element), IMessageChild
    {
        public override string? DefaultNamespace() => Namespaces.CHAT_STATES;
    }

    public class Read(XmlElement element) : Stanza(element), IMessageChild
    {
        public JID? Jid
        {
            get => Element.GetAttrJID("jid");
            set => Element.SetAttr("jid", value);
        }

        public DateTime? Timestamp
        {
            get => Element.GetAttrDate("timestamp");
            set => Element.SetAttr("timestamp", value);
        }
    }

    public class X : Stanza, IMessageChild
    {
        public List<IMessageChild> Children { get; } = [];

        public List<XmlElement> Unknown { get; } = [];

        public bool IsAnnouncement => Element.GetAttr("xmlns") == Namespaces.ANNOUNCEMENTS;

        public bool IsAttachments => Element.GetAttr("xmlns") == Namespaces.UPLOAD;

        public IEnumerable<JID> UserJoins
        {
            get
            {
                if (!IsAnnouncement) return [];

                return Children
                    .OfType<Item>()
                    .Select(t => t.Jid!)
                    .Where(t => t is not null);
            }
        }

        public IEnumerable<Img> Images => Children.OfType<Img>();

        public X(XmlElement element) : base(element)
        {
            Parse(element, Children, Unknown);
        }

        public override string? DefaultNamespace() => Namespaces.ANNOUNCEMENTS;
    }

    public class Item(XmlElement element) : Stanza(element), IMessageChild
    {
        public JID? Jid
        {
            get => Element.GetAttrJID("jid");
            set => Element.SetAttr("jid", value);
        }
    }

    public class Img(XmlElement element) : Stanza(element), IMessageChild
    {
        public string? Src
        {
            get => Element.GetAttr("src");
            set => Element.SetAttr("src", value);
        }
    }
}
