# Chat
Coming soon...

Leaving this here for now:

After reading the [documentation](./../readme.md) on how to register FaceitSharp with your service collection, you can do stuff like this:

```csharp
using FaceitSharp;

public class MessageHandler(
  IFaceitApi _api,
  ILogger<MessageHandler> _logger)
{
  public async Task Run()
  {
    //Login to the web-socket chat server
    var loggedIn = await _api.Chat.Login();
    if (!loggedIn)
    {
      _logger.LogWarning("Couldn't connect or login!");
      return
    }
    //Fetch the "hub chat" - This subscribes to the messages in the hub
    var hubChat = await _api.Chat.Messages.HubChat("<some_hub_id>");
    //Setup an observable subscription to respond to messages that mention the bot's account
    using var subscription = hubChat.Messages.Subscribe(async msg =>
    {
      //Ensure the message mentions the current user
      if (!msg.MentionsCurrentUser) return;
      //Send a message to the same chat room the message came from 
      await msg.Send($"Hello there @{msg.Author.Name} ! How's it going?", msg.Author);
    });
    //Send a message in the hub chat once the subscription is setup.
    await hubChat.Send("I'm alive!");
    //Wait forever for messages to come in
    await Task.Delay(-1);
  }
}
```