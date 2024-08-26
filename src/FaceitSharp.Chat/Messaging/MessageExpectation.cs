namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Define expectations for a message event to meet
/// </summary>
public interface IMessageExpectation
{
    /// <summary>
    /// Checks whether the message meets the expectations
    /// </summary>
    /// <param name="message">The message to check</param>
    /// <returns>Whether the message meets the expectation</returns>
    bool Matches(IMessageEvent message);
}

/// <summary>
/// Define expectations for a message event to meet
/// </summary>
/// <typeparam name="T">The type of message event</typeparam>
public class MessageExpectation<T> : IMessageExpectation
    where T : IMessageEvent
{
    private readonly List<Func<T, bool>> _conditions = [];

    /// <summary>
    /// All of the conditions currently expected
    /// </summary>
    public IEnumerable<Func<T, bool>> Conditions => _conditions;

    /// <summary>
    /// Whether or not there are conditions to meet
    /// </summary>
    public bool HasConditions => _conditions.Count > 0;

    /// <summary>
    /// Whether to require all conditions to be met or any one
    /// </summary>
    public bool And { get; set; } = true;

    /// <summary>
    /// Set whether multiple conditions should be joined by AND or OR
    /// </summary>
    /// <param name="and">Whether it should be "AND" mode</param>
    /// <returns>Current conjoiner for chaining</returns>
    public MessageExpectation<T> ConjoinMode(bool and)
    {
        And = and;
        return this;
    }

    /// <summary>
    /// Adds a condition to be met
    /// </summary>
    /// <param name="condition">The condition to meet</param>
    /// <returns>Current conjoiner for chaining</returns>
    public MessageExpectation<T> Where(Func<T, bool> condition)
    {
        _conditions.Add(condition);
        return this;
    }

    /// <summary>
    /// Checks if the message meets all of the given expectations
    /// </summary>
    /// <param name="conditions">The conditions to check</param>
    /// <returns>The current conjoiner for chaining</returns>
    public MessageExpectation<T> All(Action<MessageExpectation<T>> conditions)
    {
        var expectation = new MessageExpectation<T> { And = true };
        conditions(expectation);
        return Where(expectation.Matches);
    }

    /// <summary>
    /// Checks if the message meets any of the given expectations
    /// </summary>
    /// <param name="conditions">The conditions to check</param>
    /// <returns>The current conjoiner for chaining</returns>
    public MessageExpectation<T> Any(Action<MessageExpectation<T>> conditions)
    {
        var expectation = new MessageExpectation<T> { And = false };
        conditions(expectation);
        return Where(expectation.Matches);
    }

    /// <summary>
    /// Checks to see if the message is from a specific user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>The current conjoiner for chaining</returns>
    public MessageExpectation<T> FromUser(string userId)
    {
        return Where(t => t.Author.UserId == userId);
    }

    /// <summary>
    /// Checks to see if the message is from a specific user
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>The current conjoiner for chaining</returns>
    public MessageExpectation<T> From(FaceitUser user)
    {
        return FromUser(user.UserId);
    }

    /// <summary>
    /// Ensures the same context as the given message
    /// </summary>
    /// <param name="message">The message to compare against</param>
    /// <returns>The current conjoiner for chaining</returns>
    public MessageExpectation<T> SameAs(T message)
    {
        return Where(t => t.From == message.From && t.To == message.To && t.Context == message.Context);
    }

    /// <summary>
    /// Checks whether the message meets the expectations
    /// </summary>
    /// <param name="message">The message to check</param>
    /// <returns>Whether the message meets the expectation</returns>
    public bool Matches(T message)
    {
        if (!HasConditions) return true;

        return And
            ? _conditions.All(c => c(message))
            : _conditions.Any(c => c(message));
    }

    /// <summary>
    /// Checks whether the message meets the expectations
    /// </summary>
    /// <param name="message">The message to check</param>
    /// <returns>Whether the message meets the expectation</returns>
    public bool Matches(IMessageEvent message)
    {
        return message is T t && Matches(t);
    }
}

/// <summary>
/// Define expectations for general message events
/// </summary>
public class MessageExpectation : MessageExpectation<IMessageEvent> 
{
    /// <summary>
    /// Creates a message expectation for a specific message event
    /// </summary>
    /// <typeparam name="T">The type of message event</typeparam>
    /// <returns>The message expectation</returns>
    public static MessageExpectation<T> Create<T>() where T : IMessageEvent
    {
        return new MessageExpectation<T>();
    }
}

/// <summary>
/// Extensions for message expectations
/// </summary>
public static class MessageExpectationExtensions
{
    /// <summary>
    /// Ensures the message mentions a specific user
    /// </summary>
    /// <typeparam name="T">The type of message event</typeparam>
    /// <param name="expectation">The expectation</param>
    /// <param name="userId">The ID of the user</param>
    /// <returns>The current conjoiner for chaining</returns>
    public static MessageExpectation<T> Mentions<T>(this MessageExpectation<T> expectation, string userId)
        where T: IRoomMessage
    {
        return expectation.Where(t => t.Mentions.Any(m => m.UserId == userId));
    }

    /// <summary>
    /// Ensures the message mentions a specific user
    /// </summary>
    /// <typeparam name="T">The type of message event</typeparam>
    /// <param name="expectation">The expectation</param>
    /// <param name="user">The user</param>
    /// <returns>The current conjoiner for chaining</returns>
    public static MessageExpectation<T> Mentions<T>(this MessageExpectation<T> expectation, FaceitUser user)
        where T : IRoomMessage
    {
        return expectation.Mentions(user.UserId);
    }
}