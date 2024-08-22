namespace FaceitSharp.Cli;

using Api.Internal.Interop;

internal class StandardTest(
    IFaceitApi _api,
    IFaceitJsonService _json,
    ILogger<StandardTest> _logger) : ITest
{
    public async Task Run()
    {
        const string USERNAME = "<your username>";
        const string HUB_ID = "<your favorite hub>";

        var user = await _api.Internal.Users.ByUsername(USERNAME);
        if (user is null)
        {
            Console.WriteLine("User not found");
            return;
        }

        var hub = await _api.Internal.Hubs.Get(HUB_ID);
        if (hub is null)
        {
            Console.WriteLine("Hub not found");
            return;
        }

        var queue = (await _api.Internal.Queues.ByHub(HUB_ID))?.FirstOrDefault();
        if (queue is null)
        {
            Console.WriteLine("Queue not found");
            return;
        }

        _logger.LogInformation("[StandardTest] HUB >> {data}", _json.Pretty(hub));
        _logger.LogInformation("[StandardTest] QUEUE >> {data}", _json.Pretty(queue));
        _logger.LogInformation("[StandardTest] USER >> {data}", _json.Pretty(user));
    }
}
