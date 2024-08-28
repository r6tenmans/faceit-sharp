using System.Reactive.Linq;

namespace FaceitSharp.Cli;

using Chat;
using Chat.Messaging;
using Chat.Messaging.Rooms;

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

                if (msg.Author.Name == "Cardboard_mf" && await HandleAuthCommands(hub, msg))
                    return;

                await msg.Send($"Hello there, @{msg.Author.Name} ! How's it going in {msg.Hub.Name} today? I see your message: {msg.Context} >> {msg.Content}", msg.Author);
            });
    }

    public async Task<bool> HandleAuthCommands(IHubRoom room, IHubReplyMessage message)
    {
        var mentions = message
            .Mentions
            .Where(t => t.UserId != _chat.Auth.Profile.UserId && t.UserId != message.Author.UserId)
            .DistinctBy(t => t.UserId)
            .ToArray();

        var words = message.Content.Words();
        if (words.Contains("mute"))
        {
            var results = await Task.WhenAll(
                mentions.Select(t => room.Mute(t.UserId, TimeSpan.FromDays(365 * 10))));
            await message.Send(
                $"@{message.Author.Name} Muted {string.Join(" , ", mentions.Select(t => "@" + t.Name))} for 10 years", 
                [message.Author, ..mentions]);
            return true;
        }

        if (words.Contains("unmute"))
        {
            var results = await Task.WhenAll(
                mentions.Select(t => room.Unmute(t.UserId)));
            await message.Send(
                $"@{message.Author.Name} Unmuted {string.Join(" , ", mentions.Select(t => "@" + t.Name))}",
                [message.Author, ..mentions]);
            return true;
        }

        return false;
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
