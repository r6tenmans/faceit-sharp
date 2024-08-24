namespace FaceitSharp.Chat.XMPP;

/// <summary>
/// Represents the address of an XMPP entity, also known as Jabber Identifier (JID).
/// </summary>
[Serializable]
public partial class JID
{
    /// <summary>
    /// The domain identifier of the JID.
    /// </summary>
    public string Domain
    {
        get;
        private set;
    }

    /// <summary>
    /// The node identifier of the JID. This may be null or empty.
    /// </summary>
    public string? Node
    {
        get;
        private set;
    }

    /// <summary>
    /// The resource identifier of the JID. This may be null or empty.
    /// </summary>
    public string? Resource
    {
        get;
        private set;
    }

    /// <summary>
    /// Determines whether the JID is a 'bare JID', i.e. a JID without resource identifier.
    /// </summary>
    public bool IsBareJID
    {
        get
        {
            return
                !string.IsNullOrEmpty(Node) &&
                !string.IsNullOrEmpty(Domain) &&
                string.IsNullOrEmpty(Resource);
        }
    }

    /// <summary>
    /// Determines whether the JID is a 'full JID', i.e. a JID with both a node and a resource identifier.
    /// </summary>
    public bool IsFullJID
    {
        get
        {
            return
                !string.IsNullOrEmpty(Node) &&
                !string.IsNullOrEmpty(Domain) &&
                !string.IsNullOrEmpty(Resource);
        }
    }

    /// <summary>
    /// Initializes a new instance of the JID class.
    /// </summary>
    /// <param name="jid">A string from which to construct the JID.</param>
    /// <exception cref="ArgumentNullException">The jid parameter is null.</exception>
    /// <exception cref="ArgumentException">The jid parameter does not represent a valid JID.</exception>
    public JID(string jid)
    {
        ArgumentException.ThrowIfNullOrEmpty(jid, nameof(jid));
        Match m = JIDRegex().Match(jid);
        if (!m.Success)
            throw new ArgumentException("The argument is not a valid JID.");
        Domain = m.Groups["domain"].Value;
        Node = m.Groups["node"].Value;
        if (Node == string.Empty)
            Node = null;
        Resource = m.Groups["resource"].Value;
        if (Resource == string.Empty)
            Resource = null;
    }

    /// <summary>
    /// Initializes a new instance of the JID class using the specified domain, node and optionally resource.
    /// </summary>
    /// <param name="domain">The domain of the JID.</param>
    /// <param name="node">The node of the JID.</param>
    /// <param name="resource">The resource of the JID. This may be omitted.</param>
    /// <exception cref="ArgumentNullException">The domain parameter is null.</exception>
    /// <exception cref="ArgumentException">The domain parameter is the empty string.</exception>
    public JID(string domain, string? node = null, string? resource = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(domain, nameof(domain));
        Domain = domain;
        Node = node;
        Resource = resource;
    }

    /// <summary>
    /// Implicit conversion operator for type string to type JID.
    /// </summary>
    /// <param name="jid">The string to convert into a JID instance.</param>
    /// <returns>A JID instance created from the specified string.</returns>
    public static implicit operator JID?(string? jid)
    {
        return string.IsNullOrEmpty(jid) ? null : new JID(jid);
    }

    /// <summary>
    /// Returns a new JID instance representing the 'bare JID' constructed from
    /// this JID.
    /// </summary>
    /// <returns>A bare JID constructed from this JID instance.</returns>
    public JID GetBareJID()
    {
        return new JID(Domain, Node);
    }

    /// <summary>
    /// Returns a textual representation of the JID.
    /// </summary>
    /// <returns>A textual representation of this JID instance.</returns>
    public override string ToString()
    {
        var b = new StringBuilder();
        if (!string.IsNullOrEmpty(Node))
            b.Append(Node + "@");
        b.Append(Domain);
        if (!string.IsNullOrEmpty(Resource))
            b.Append("/" + Resource);
        return b.ToString();
    }

    /// <summary>
    /// Determines whether the specified object is equal to this JID instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is semantically equal to this JID instance; Otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is not JID other || other is null)
            return false;

        return
            Node == other.Node &&
            Domain == other.Domain &&
            Resource == other.Resource;
    }

    /// <summary>
    /// Returns the hash code of this instance.
    /// </summary>
    /// <returns>The hash code of this JID instance.</returns>
    public override int GetHashCode()
    {
        int hash = 13;
        if (Node != null)
            hash = (hash * 7) + Node.GetHashCode();
        hash = (hash * 7) + Domain.GetHashCode();
        if (Resource != null)
            hash = (hash * 7) + Resource.GetHashCode();
        return hash;
    }

    /// <summary>
    /// Determines whether the specified JID objects are equal.
    /// </summary>
    /// <param name="a">The first object.</param>
    /// <param name="b">The second object.</param>
    /// <returns>True if the specified objects are semantically equal; Otherwise false.</returns>
    public static bool operator ==(JID? a, JID? b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if ((a is null) || (b is null))
            return false;
        return a.Node == b.Node && a.Domain == b.Domain &&
            a.Resource == b.Resource;
    }

    /// <summary>
    /// Determines whether the specified JID objects are unequal.
    /// </summary>
    /// <param name="a">The first object.</param>
    /// <param name="b">The second object.</param>
    /// <returns>True if the specified objects are not semantically equal; Otherwise false.</returns>
    public static bool operator !=(JID? a, JID? b)
    {
        return !(a == b);
    }

    [GeneratedRegex("(?:(?<node>[^@]+)@)?(?<domain>[^/]+)(?:/(?<resource>.+))?")]
    private static partial Regex JIDRegex();
}
