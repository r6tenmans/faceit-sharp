namespace FaceitSharp.Chat.Network;

public class EdgeSocket(
    ILogger<EdgeSocket> _logger,
    IFaceitConfig _config) : Socket(_logger)
{
    public const string DEFAULT_URI = "wss://edge.faceit.com/v1/ws";
    public const int DEFAULT_KEEP_ALIVE = 35;
    public const int DEFAULT_RECONNECT = 10;
    public const int DEFAULT_RECONNECT_ERROR = 30;

    public override string Name => "FaceIT Edge";

    public async Task<bool> Connect()
    {
        var result = await Connect(_config.Edge);

        return result;
    }
}
