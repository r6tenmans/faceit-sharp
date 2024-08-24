namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Features : Stanza
{
    public List<IFeatureChild> Children { get; } = [];

    public List<XmlElement> Unknown { get; } = [];

    public IEnumerable<Bind> Binds => Children.OfType<Bind>();

    public IEnumerable<Session> Sessions => Children.OfType<Session>();

    public IEnumerable<Sm> Sms => Children.OfType<Sm>();

    public IEnumerable<string> AuthMechanisms => Children.OfType<Mechanisms>().SelectMany(t => t.Mechanism);

    public Features(XmlElement element) : base(element)
    {
        foreach(var obj in element.ChildNodes)
        {
            if (obj is not XmlElement child) continue;

            var stanza = Xml.GetStanza(child, 
                typeof(Bind), typeof(Session), typeof(Sm), typeof(Mechanisms));

            if (stanza is null || stanza is not IFeatureChild feature)
            {
                Unknown.Add(child);
                continue;
            }

            Children.Add(feature);
        }
    }

    public interface IFeatureChild { }

    public class Bind : Stanza, IFeatureChild { }

    public class Session : Stanza, IFeatureChild { }

    public class Sm : Stanza, IFeatureChild { }

    public class Mechanisms : Stanza, IFeatureChild
    {
        public List<string> Mechanism { get; } = [];

        public Mechanisms(XmlElement element) : base(element)
        {
            foreach (var child in element.ChildNodes)
            {
                if (child is not XmlElement value) continue;

                Mechanism.Add(value.InnerText);
            }
        }
    }
}
