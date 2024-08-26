namespace FaceitSharp.Core;

/// <summary>
/// Settings related to webhook configs
/// </summary>
public class FaceitConfigWebhook()
{
    /// <summary>
    /// Log all of the webhook events that are processed
    /// </summary>
    public bool LogHooks { get; set; } = true;

    /// <summary>
    /// Settings related to webhook configs
    /// </summary>
    /// <param name="section">The configuration section to draw from</param>
    public FaceitConfigWebhook(IConfigurationSection section) : this()
    {
        LogHooks = section.GetValue(nameof(LogHooks), LogHooks);
    }
}
