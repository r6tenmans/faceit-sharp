using System.Security;

namespace FaceitSharp.Chat.XMPP;

/// <summary>
/// Utility for managing XML elements.
/// </summary>
public static partial class Xml
{
    /// <summary>
    /// Creates a new XmlElement instance.
    /// </summary>
    /// <param name="name">The name of the element.</param>
    /// <param name="namespace">The namespace of the element.</param>
    /// <returns>An initialized instance of the XmlElement class.</returns>
    /// <exception cref="ArgumentNullException">The name parameter is null.</exception>
    /// <exception cref="ArgumentException">The name parameter is the empty string.</exception>
    /// <exception cref="XmlException">The name or the namespace parameter is invalid.</exception>
    public static XmlElement Element(string name, string? @namespace = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        return new XmlDocument().CreateElement(name, @namespace);
    }

    /// <summary>
    /// Adds the specified element to the end of the list of child nodes, of this node.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="child">The node to add.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement Child(this XmlElement e, XmlElement child)
    {
        XmlNode imported = e.OwnerDocument.ImportNode(child, true);
        e.AppendChild(imported);
        return e;
    }

    /// <summary>
    /// Sets the value of the attribute with the specified name.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement Attr(this XmlElement e, string name, string value)
    {
        e.SetAttribute(name, value);
        return e;
    }

    /// <summary>
    /// Adds the specified text to the end of the list of child nodes, of this node.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="text">The text to add.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement Text(this XmlElement e, string text)
    {
        e.AppendChild(e.OwnerDocument.CreateTextNode(text));
        return e;
    }

    /// <summary>
    /// Serializes the XmlElement instance into a string.
    /// </summary>
    /// <param name="e">The XmlElement instance the method is invoked for.</param>
    /// <param name="xmlDeclaration">true to include a XML declaration, otherwise false.</param>
    /// <param name="leaveOpen">true to leave the tag of an empty element open, otherwise false.</param>
    /// <returns>A textual representation of the XmlElement instance.</returns>
    public static string ToXmlString(this XmlElement e, bool xmlDeclaration = false, bool leaveOpen = false)
    {
        // Can't use e.OuterXml because it "messes up" namespaces for elements with
        // a prefix, i.e. stream:stream (What it does is probably correct, but just
        // not what we need for XMPP).
        var b = new StringBuilder("<" + e.Name);
        if (!string.IsNullOrEmpty(e.NamespaceURI))
            b.Append(" xmlns='" + e.NamespaceURI + "'");
        foreach (XmlAttribute a in e.Attributes)
        {
            if (a.Name == "xmlns")
                continue;
            if (a.Value != null)
                b.Append(" " + a.Name + "='" + SecurityElement.Escape(a.Value.ToString())
                    + "'");
        }
        if (e.IsEmpty)
            b.Append("/>");
        else
        {
            b.Append('>');
            foreach (var child in e.ChildNodes)
            {
                if (child is XmlElement element)
                    b.Append(element.ToXmlString());
                else if (child is XmlText text)
                    b.Append(text.InnerText);
            }
            b.Append("</" + e.Name + ">");
        }
        string xml = b.ToString();
        if (xmlDeclaration)
            xml = "<?xml version='1.0' encoding='UTF-8'?>" + xml;
        if (leaveOpen)
            return XMLClose().Replace(xml, ">");
        return xml;
    }

    /// <summary>
    /// Deserializes the XML into an XmlElement instance.
    /// </summary>
    /// <param name="xml">The XML data as a string</param>
    /// <returns>The XML element that was deserialized</returns>
    public static XmlElement? FromXmlString(this string? xml)
    {
        xml = xml.ForceNull();
        if (xml is null) return null;

        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return doc.FirstChild as XmlElement;
    }

    /// <summary>
    /// Gets an instance of the stanza from the given XmlElement.
    /// </summary>
    /// <param name="element">The XML element to parse</param>
    /// <param name="types">The types of stanzas it could be</param>
    /// <returns>The stanza instance</returns>
    public static Stanza? GetStanza(this XmlElement element, params Type[] types)
    {
        var type = types.FirstOrDefault(t => t.Name.Equals(element.Name, StringComparison.CurrentCultureIgnoreCase));
        if (type is null) return null;

        var ctor = type.GetConstructor([typeof(XmlElement)]);
        if (ctor is null) return null;

        var instance = ctor.Invoke(new object[] { element });
        return instance as Stanza;
    }

    /// <summary>
    /// Sets or removes the attribute with the specified name.
    /// </summary>
    /// <param name="element">The XmlElement instance the method is invoked for.</param>
    /// <param name="name">The name of the attribute to create or alter.</param>
    /// <param name="value">The value to set for the attribute.</param>
    /// <returns>A reference to the XmlElement instance.</returns>
    public static XmlElement SetAttr(this XmlElement element, string name, string? value)
    {
        value = value?.ForceNull();
        if (value is null)
            element.RemoveAttribute(name);
        else
            element.SetAttribute(name, value);
        return element;
    }

    public static XmlElement SetAttr(this XmlElement element, string name, int? value)
    {
        return element.SetAttr(name, value?.ToString());
    }

    public static XmlElement SetAttr(this XmlElement element, string name, DateTime? value)
    {
        return element.SetAttr(name, value?.ToString("O"));
    }

    public static XmlElement SetAttr(this XmlElement element, string name, JID? value)
    {
        return element.SetAttr(name, value?.ToString());
    }

    public static XmlElement SetAttr<T>(this XmlElement element, string name, T? value)
        where T: struct, Enum, IComparable, IFormattable, IConvertible
    {
        return element.SetAttr(name, value?.ToString());
    }

    public static string? GetAttr(this XmlElement element, string name)
    {
        return element.GetAttribute(name)?.ForceNull();
    }

    public static int? GetAttrInt(this XmlElement element, string name)
    {
        var value = element.GetAttr(name);
        return int.TryParse(value, out var result) ? result : null;
    }

    public static DateTime? GetAttrDate(this XmlElement element, string name)
    {
        var value = element.GetAttr(name);
        return DateTime.TryParse(value, out var result) ? result : null;
    }
    
    public static JID? GetAttrJID(this XmlElement element, string name)
    {
        var value = element.GetAttr(name);
        return (JID?)value;
    }

    public static T? GetAttr<T>(this XmlElement element, string name)
        where T: struct, Enum, IComparable, IFormattable, IConvertible
    {
        var value = element.GetAttr(name);
        return Enum.TryParse<T>(value, true, out var result) ? result : null;
    }

    [GeneratedRegex("/>$")]
    private static partial Regex XMLClose();
}
