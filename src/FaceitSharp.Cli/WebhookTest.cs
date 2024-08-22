using FaceitSharp.Webhooks;

namespace FaceitSharp.Cli;

internal class WebhookTest(IFaceitApi _api) : ITest
{
    public async Task Run()
    {
        var hooks = new string[]
        {
            //Put your webhook JSON data here
        };

        foreach(var hook in hooks)
        {
            await _api.Webhooks.Handle(hook);
        }
    }
}

internal class WebhookTestHandler(
    IFaceitApi _api,
    ILogger<WebhookTestHandler> _logger) : FaceitWebhookEventHandler
{
    public override async Task HubUserAdded(FaceitWebhookDetails webhook, EventHubUserAdded user)
    {
        var profileTask = _api.Internal.Users.ById(user.UserId);
        var hubTask = _api.Internal.Hubs.Get(user.Id);

        await Task.WhenAll(profileTask, hubTask);

        var profile = await profileTask;
        var hub = await hubTask;

        if (profile is null || hub is null)
        {
            _logger.LogError("Failed to get profile ({pm}) and/or hub ({ph})",
                profile is null ? "missing" : "found",
                hub is null ? "missing" : "found");
            return;
        }

        var username = profile.Name;
        var hubName = hub.Name;

        var roles = user.Roles
            .Select(r => hub.Roles.FirstOrDefault(hr => hr.Id == r))
            .Where(r => r != null)
            .Select(r => r!.Name)
            .ToArray();

        _logger.LogInformation("User {username} joined hub {hubName} with roles: {roles}", username, hubName, string.Join(", ", roles));
    }

}