using System.Reactive.Linq;

namespace FaceitSharp.Cli;

using Chat;
using Chat.Messaging;
using Chat.Messaging.Rooms;
using Chat.XMPP;

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

        using var sub3 = _chat.Messages.All.Subscribe(msg =>
        {
            _logger.LogWarning("MSG >> {type} >> [{time:yyyy-MM-dd HH:mm:ss}] {Name}: {Content}",
                    msg.Context.ToString(), msg.Timestamp, msg.Author.Name, msg.Content);
        });

        using var sub = _chat.Messages.MessageDeleted.Subscribe(t =>
        {
            _logger.LogWarning("MSG DELETED >> [{time:yyyy-MM-dd HH:mm:ss}] {context}: {messageId}", 
                t.Timestamp, t.Context.ToString(), t.MessageId);
        });

        using var sub2 = _chat.Messages.MessageEdited.Subscribe(t =>
        {
            _logger.LogWarning("MSG EDITED >> [{time:yyyy-MM-dd HH:mm:ss}] {context}: {messageId}", 
                t.Edited, t.Context.ToString(), t.MessageId);
        });

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
                _logger.LogWarning("MSG >> HUB >> [{time:yyyy-MM-dd HH:mm:ss}] {Name}: {Content}", 
                    msg.Timestamp, msg.Author.Name, msg.Content);

                if (msg.Edited is not null)
                    _logger.LogWarning("MSG >> HUB >> [{time:yyyy-MM-dd HH:mm:ss}] {Name} edited: {Content}", 
                                               msg.Edited, msg.Author.Name, msg.Content);

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

        if (words.Contains("delete"))
        {
            var res = await message.Delete();
            _logger.LogWarning("Deleted message: {res}", res.Element.ToXmlString());
            return true;
        }

        if (words.Contains("edit"))
        {
            var msg = await message.Send("Testing");
            await Task.Delay(1000);
            var res = await _chat.Messages.Edit(msg, "Testing 2");
            _logger.LogWarning("Edited message: {res}", res.Element.ToXmlString());
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
                _logger.LogWarning("MSG >> MATCH >> [{time:yyyy-MM-dd HH:mm:ss}] {Name}: {Content}", 
                    msg.Timestamp, msg.Author.Name, msg.Content);

                if (msg.Edited is not null)
                    _logger.LogWarning("MSG >> HUB >> [{time:yyyy-MM-dd HH:mm:ss}] {Name} edited: {Content}",
                        msg.Edited, msg.Author.Name, msg.Content);

                if (!msg.MentionsCurrentUser) return;

                await msg.Send($"Hello there, @{msg.Author.Name} ! How is the match going? (Sorry, I can't do anything yet!)", msg.Author);
            });
    }
}
