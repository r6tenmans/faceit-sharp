namespace FaceitSharp.Chat.Messaging;

/// <summary>
/// Provides utility methods for working with messages
/// </summary>
public static class MessageUtil
{
    /// <summary>
    /// Parses the context from the given JID
    /// </summary>
    /// <param name="jid">The JID to parse</param>
    /// <param name="matchId">The ID of the match for this context</param>
    /// <param name="teamId">The ID of the team for this context</param>
    /// <param name="hubId">The ID of the hub for this context</param>
    /// <param name="userId">The ID of the user for this context</param>
    /// <returns>The parsed context type</returns>
    public static ContextType Context(this JID jid, out string? matchId, out string? teamId, out string? hubId, out string? userId)
    {
        matchId = null;
        teamId = null;
        hubId = null;
        userId = jid.Resource;

        if (string.IsNullOrEmpty(jid.Node)) return ContextType.Unknown;

        string id;
        if (jid.Node.StartsWith("team-", StringComparison.InvariantCultureIgnoreCase))
        {
            id = jid.Node[5..];
            if (!id.Contains('_')) return ContextType.Unknown;

            var parts = id.Split('_');
            matchId = parts.First();
            teamId = parts.Last();
            return ContextType.Team;
        }

        if (jid.Node.StartsWith("hub-", StringComparison.InvariantCultureIgnoreCase))
        {
            hubId = jid.Node[4..].Replace("-general", "");
            return ContextType.Hub;
        }

        if (jid.Node.StartsWith("match-", StringComparison.InvariantCultureIgnoreCase))
        {
            matchId = jid.Node[6..];
            return ContextType.Match;
        }

        return ContextType.Unknown;
    }

    /// <summary>
    /// Gets the JID for the given match
    /// </summary>
    /// <param name="match">The match</param>
    /// <returns>The matches JID</returns>
    public static JID GetJID(this FaceitMatch match)
    {
        return new("conference.faceit.com", "match-" + match.Id);
    }

    /// <summary>
    /// Gets the JIDs for the given match
    /// </summary>
    /// <param name="match">The match</param>
    /// <returns>The match JID, and the teams JIDs</returns>
    public static (JID match, JID? leftTeam, JID? rightTeam) GetJIDs(this FaceitMatch match)
    {
        var matchJid = match.GetJID();
        var left = match.Teams.Values.FirstOrDefault()?.GetJID(match);
        var right = match.Teams.Values.LastOrDefault()?.GetJID(match);
        return (matchJid, left, right);
    }

    /// <summary>
    /// Gets the JID for the given team
    /// </summary>
    /// <param name="team">The team</param>
    /// <param name="match">The match</param>
    /// <returns>The team's JID</returns>
    public static JID GetJID(this FaceitTeam team, FaceitMatch match)
    {
        return new("conference.faceit.com", $"team-{match.Id}_{team.Id}");
    }

    /// <summary>
    /// Gets the JID for the given hub
    /// </summary>
    /// <param name="hub">The hub</param>
    /// <returns>The hub's JID</returns>
    public static JID GetJID(this FaceitHub hub)
    {
        return new("conference.faceit.com", $"hub-{hub.Guid}-general");
    }

    /// <summary>
    /// Parses the mentions and their positions from the given message
    /// </summary>
    /// <param name="message">The message to parse through</param>
    /// <param name="mentions">The mentions to find</param>
    /// <returns>The parsed mentions</returns>
    public static IEnumerable<MessageMention> ParseMentions(this string message, params UserMention[] mentions)
    {
        foreach (var mention in mentions)
        {
            var textToFind = mention.Mention;
            var resourceId = mention.ResourceId;

            int startIndex = 0;
            int index;
            while ((index = message.IndexOf(textToFind, startIndex)) != -1)
            {
                var end = index + textToFind.Length;
                yield return new MessageMention(resourceId, index, end);
                startIndex = end;
            }
        }
    }
}
