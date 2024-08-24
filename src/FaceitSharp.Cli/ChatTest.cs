namespace FaceitSharp.Cli;

using Chat;
using Chat.Rooms;
using Chat.XMPP;
using System.Reactive.Linq;

internal class ChatTest(
    IFaceitChat _chat,
    IFaceitApi _api,
    ILogger<ChatTest> _logger) : ITest
{
    public async Task Run()
    {
        var worked = await _chat.Login();
        if (!worked)
        {
            _logger.LogError("Failed to login");
            return;
        }

        const string matchId = "1-c87425e6-8405-4aca-9c35-dbf2a1bb4434";

        var match = await _chat.GetMatch(matchId);
        using var subscription = match.MessageReceived
            .Subscribe(async t =>
            {
                _logger.LogInformation("[MATCH MESSAGE RECEVIED::{id}] >> {type} >> {data}",
                    matchId, t.GetType().Name, t.Element.ToXmlString());

                var userId = t.From?.Resource;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("No user id found in stanza");
                    return;
                }

                var user = await _api.Internal.Users.ById(userId);
                if (user is null)
                {
                    _logger.LogWarning("User not found: {id}", userId);
                    return;
                }

                await match.Send($"Hello there, @{user.Name}! How are you doing? I hope you're doing well @{user.Name}!", new UserMention(user.Name, user.UserId));
            });

        _logger.LogInformation("Waiting for messages");
        await Task.Delay(-1);
    }
}
