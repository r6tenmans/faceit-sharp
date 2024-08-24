namespace FaceitSharp.Chat.XMPP;

public static class X
{
    public static XmlElement C(string name, string? @namespace, (string name, string? value)[] attributes, params XmlElement[] children)
    {
        var e = Xml.Element(name, @namespace);
        foreach (var (n, v) in attributes)
            e.SetAttr(n, v);
        foreach (var c in children)
            e.Child(c);
        return e;
    }

    public static XmlElement C(string name, string? @namespace, (string name, string? value)[] attributes, string text)
    {
        var e = Xml.Element(name, @namespace);
        foreach (var (n, v) in attributes)
            e.SetAttr(n, v);
        e.InnerText = text;
        return e;
    }

    public static XmlElement C(string name, string? @namespace, (string name, string? value)[] attributes)
    {
        var e = Xml.Element(name, @namespace);
        foreach (var (n, v) in attributes)
            e.SetAttr(n, v);
        return e;
    }
}
