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

    public Message(XmlElement element) : base(element)
    {
        foreach (var obj in element.ChildNodes)
        {
            if (obj is not XmlElement child) continue;

            var stanza = Xml.GetStanza(child,
                typeof(Body), typeof(Data), 
                typeof(Archived), typeof(Reference),
                typeof(Composing), typeof(Read),
                typeof(X));

            if (stanza is null || stanza is not IMessageChild el)
            {
                Unknown.Add(child);
                continue;
            }

            Children.Add(el);
        }
    }

    public override string? DefaultNamespace() => Namespaces.CLIENT;

    public interface IMessageChild { }

    public class Body : Stanza, IMessageChild
    {
        public string? Value
        {
            get => Element.InnerText?.ForceNull();
            set => Element.InnerText = value ?? string.Empty;
        }
    }

    public class Data : Stanza, IMessageChild
    {
        public DateTime? Timestamp
        {
            get => Element.GetAttrDate("timestamp");
            set => Element.SetAttr("timestamp", value);
        }
    }

    public class Archived : Stanza, IMessageChild
    {
        public JID? By
        {
            get => Element.GetAttrJID("by");
            set => Element.SetAttr("by", value);
        }
    }

    public class Reference : Stanza, IMessageChild
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

    public class Composing : Stanza, IMessageChild
    {
        public override string? DefaultNamespace() => Namespaces.CHAT_STATES;
    }

    public class Read : Stanza, IMessageChild
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
        public List<JID> Items { get; } = [];

        public List<XmlElement> Unknown { get; } = [];

        public X(XmlElement element) : base(element)
        {
            foreach(var obj in element.ChildNodes)
            {
                if (obj is not XmlElement child) continue;

                if (child.Name != "item")
                {
                    Unknown.Add(child);
                    continue;
                }

                var jid = child.GetAttrJID("jid");
                if (jid is not null) Items.Add(jid);
            }
        }

        public override string? DefaultNamespace() => Namespaces.ANNOUNCEMENTS;
    }
}
