namespace FaceitSharp.Core;

/// <summary>
/// Settings for faceit-sharp
/// </summary>
public class FaceitConfig()
{
    /// <summary>
    /// The internal API settings
    /// </summary>
    public FaceitConfigInternalApi Internal { get; set; } = new FaceitConfigInternalApi();

    /// <summary>
    /// The webhook settings
    /// </summary>
    public FaceitConfigWebhook Webhooks { get; set; } = new FaceitConfigWebhook();

    /// <summary>
    /// The chat settings
    /// </summary>
    public FaceitConfigChat Chat { get; set; } = new FaceitConfigChat();

    /// <summary>
    /// Settings for faceit-sharp
    /// </summary>
    /// <param name="section">The configuration section to draw from</param>
    public FaceitConfig(IConfigurationSection section): this()
    {
        Internal = new(section.GetSection(nameof(Internal)));
        Webhooks = new(section.GetSection(nameof(Webhooks)));
        Chat = new(section.GetSection(nameof(Chat)));
    }
}