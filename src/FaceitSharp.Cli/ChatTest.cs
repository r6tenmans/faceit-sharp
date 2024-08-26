using System.Reactive.Linq;

namespace FaceitSharp.Cli;

using Chat;
using FaceitSharp.Chat.Messaging;

internal class ChatTest(
    IFaceitChatClient _chat,
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

        const string matchId = "1-7b81581e-6913-4829-9b34-4dffb9420560";
        const string hubId = "fc88246e-f1a0-46ab-a1ec-0fe1e418db7b";

        using var match = await WatchMatch(matchId);
        using var hub = await WatchHub(hubId);

        _logger.LogInformation("Waiting for messages");
        await Task.Delay(-1);
    }

    public async Task<IDisposable> WatchHub(string id)
    {
        var hub = await _chat.Messages.HubChat(id);
        return hub
            .Messages
            .Subscribe(async msg =>
            {
                _logger.LogInformation("MSG >> HUB >> [{time:yyyy-MM-dd HH:mm:ss}] {Name}: {Content}", 
                    msg.Timestamp, msg.Author.Name, msg.Content);

                if (!msg.MentionsCurrentUser) return;

                await msg.Send($"Hello there, @{msg.Author.Name} ! How's it going in {msg.Hub.Name} today? (Sorry, I can't do anything yet!)", msg.Author);
            });
    }

    public async Task<IDisposable> WatchMatch(string id)
    {
        var match = await _chat.Messages.MatchRoom(id);
        return match
            .Messages
            .Subscribe(async msg =>
            {
                _logger.LogInformation("MSG >> MATCH >> [{time:yyyy-MM-dd HH:mm:ss}] {Name}: {Content}", 
                    msg.Timestamp, msg.Author.Name, msg.Content);
                
                if (!msg.MentionsCurrentUser) return;

                await msg.Send($"Hello there, @{msg.Author.Name} ! How is the match going? (Sorry, I can't do anything yet!)", msg.Author);
            });
    }
}
