namespace FaceitSharp.Chat.XMPP;

public interface IResponseExpected
{
    /// <summary>
    /// All of the types that are expected to be matched
    /// </summary>
    IEnumerable<Type> UniqueTypes { get; }

    /// <summary>
    /// There are at least one condition to check
    /// </summary>
    bool HasConditions { get; }

    /// <summary>
    /// Set whether multiple conditions should be joined by AND or OR
    /// </summary>
    /// <param name="and">Whether it should be "AND" mode</param>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected ConjoinMode(bool and);

    /// <summary>
    /// Checks if the stanza matches the conditions
    /// </summary>
    /// <param name="condition">The predicate for the condition</param>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected Where(Func<Stanza, bool> condition);

    /// <summary>
    /// Stanza should be of a specific type and match the given conditions
    /// </summary>
    /// <typeparam name="T">The type of stanza</typeparam>
    /// <param name="condition">The predicate for the condition</param>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected Where<T>(Func<T, bool> condition) where T : Stanza;

    /// <summary>
    /// A sub-set of conditions that should all be met
    /// </summary>
    /// <param name="conditions">The conditions to check</param>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected All(Action<IResponseExpected> conditions);

    /// <summary>
    /// A sub-set of conditions where any one of them should be met
    /// </summary>
    /// <param name="conditions">The conditions to check</param>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected Any(Action<IResponseExpected> conditions);

    /// <summary>
    /// Stanza should be of a specific type
    /// </summary>
    /// <typeparam name="T">The type of stanza</typeparam>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected ShouldBe<T>() where T : Stanza;

    /// <summary>
    /// Stanza should be of a specific type
    /// </summary>
    /// <param name="type">The type of stanza</param>
    /// <returns>Current conjoiner for chaining</returns>
    IResponseExpected ShouldBe(Type type);

    /// <summary>
    /// Checks whether the given stanza matches the conditions
    /// </summary>
    /// <param name="stanza">The stanza to check</param>
    /// <returns>Whether the stanza matches the conditions</returns>
    bool Matches(Stanza stanza);
}

public class ResponseExpected : IResponseExpected
{
    public List<Func<Stanza, bool>> Conditions { get; } = [];

    public List<Type> TypeChecks { get; } = [];

    public IEnumerable<Type> UniqueTypes => TypeChecks.Distinct();

    public bool HasConditions => Conditions.Count > 0;

    public bool And { get; set; } = false;

    public IResponseExpected ConjoinMode(bool and)
    {
        And = and;
        return this;
    }

    public IResponseExpected Where(Func<Stanza, bool> condition)
    {
        Conditions.Add(condition);
        return this;
    }

    public IResponseExpected Where<T>(Func<T, bool> condition) where T : Stanza
    {
        TypeChecks.Add(typeof(T));
        return Where(s => s is T t && condition(t));
    }

    public IResponseExpected All(Action<IResponseExpected> conditions)
    {
        var responseExpected = new ResponseExpected
        {
            And = true
        };
        conditions(responseExpected);
        TypeChecks.AddRange(responseExpected.TypeChecks);
        return Where(responseExpected.Matches);
    }

    public IResponseExpected Any(Action<IResponseExpected> conditions)
    {
        var responseExpected = new ResponseExpected
        {
            And = false
        };
        conditions(responseExpected);
        TypeChecks.AddRange(responseExpected.TypeChecks);
        return Where(responseExpected.Matches);
    }

    public IResponseExpected ShouldBe<T>() where T : Stanza
    {
        return ShouldBe(typeof(T));
    }

    public IResponseExpected ShouldBe(Type type)
    {
        TypeChecks.Add(type);
        return Where(s => s.NodeName().Equals(type.Name, StringComparison.CurrentCultureIgnoreCase));
    }

    public bool Matches(Stanza stanza)
    {
        if (Conditions.Count == 0) return true;

        return And 
            ? Conditions.All(c => c(stanza))
            : Conditions.Any(c => c(stanza));
    }
}
