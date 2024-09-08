namespace FaceitSharp.Chat.XMPP.Stanzas;

public class Presence : Stanza
{
    /// <summary>
    /// The type of presence result
    /// </summary>
    public string? Type
    {
        get => Element.GetAttr("type");
        set => Element.SetAttr("type", value);
    }

    /// <summary>
    /// Initializes a new instance of the Presence class.
    /// </summary>
    /// <param name="data">The content of the stanza.</param>
    public Presence(params XmlElement?[] data) : base(data) { }

    /// <summary>
    /// Initializes a new instance of the Presence class.
    /// </summary>
    /// <param name="element">The XML element</param>
    public Presence(XmlElement element) : base(element) { }

    public override string? DefaultNamespace() => Namespaces.CLIENT;
}
