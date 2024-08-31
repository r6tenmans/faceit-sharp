namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Represents a user mention for a message 
/// </summary>
/// <param name="Username">The name of the user being mentioned</param>
/// <param name="UserId">The ID of the user being mentioned</param>
public record class UserMention(string Username, string UserId)
{
    /// <summary>
    /// Whether or not the mention is for @everyone
    /// </summary>
    public bool IsEveryone => UserId.Equals("everyone", StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    /// Whether or not the mention is for @here
    /// </summary>
    public bool IsHere => UserId.Equals("here", StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    /// Gets the resource ID for the mention
    /// </summary>
    public string ResourceId => IsEveryone
        ? "room:everyone"
        : IsHere
            ? "room:here"
            : $"xmpp:{UserId}@faceit.com";

    /// <summary>
    /// Gets the slug for the mention
    /// </summary>
    public string Mention => $"@{Username}";

    /// <summary>
    /// Implicitly convert a <see cref="FaceitPartialUser"/> to a <see cref="UserMention"/>
    /// </summary>
    /// <param name="user">The user to convert to a mention</param>
    public static implicit operator UserMention(FaceitPartialUser user) => new(user.Name, user.UserId);
}

/// <summary>
/// Represents a mention to everyone.
/// </summary>
public record class EveryoneMention() : UserMention("everyone", "everyone");

/// <summary>
/// Represents a mention to everyone currently present
/// </summary>
public record class HereMention(): UserMention("here", "here");