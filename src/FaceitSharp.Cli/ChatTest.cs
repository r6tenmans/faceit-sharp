namespace FaceitSharp.Cli;

using Chat;
using Chat.XMPP;

internal class ChatTest(
    IFaceitChat _chat,
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

        (await _chat.OnMatch(matchId))
            .Subscribe(t =>
            {
                _logger.LogInformation("[MATCH STANZA RECEVIED::{id}] >> {type} >> {data}",
                    matchId, t.GetType().Name, t.Element.ToXmlString());
            });

        await Task.Delay(-1);
    }
}
