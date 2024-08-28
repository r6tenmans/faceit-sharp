# Configuration Options
These are all of the properties on the `FaceitConfig` class under the namespace `FaceitSharp.Core`.
This class contains all of the configuration options for every service under the `FaceitSharp` library.

| Configuration Path            | Default Value                 | Data Type                 | Description | 
| ----------------------------- | ----------------------------- | ------------------------- | --- |
| Faceit                        | `new()`                       | `FaceitConfig`            | All of the settings for FaceitSharp |
| Faceit:Internal               | `new()`                       | `FaceitConfigInternalApi` | Settings related to the Internal FaceIT API |
| Faceit:Internal:Url           | `"https://api.faceit.com"`    | `string`                  | The URL to use for all internal API requests |
| Faceit:Internal:ThrowErrors   | `false`                       | `bool`                    | Whether or not to re-throw handled API errors |
| Faceit:Internal:Token[^f]     | `string.Empty`                | `Func<Task<string>>`      | The factory for the internal API token |
| Faceit:Internal:UserAgent[^f] | `"Faceit-Sharp/v1.0"`         | `Func<Task<string>>`      | The factory for the internal user-agent |
| Faceit:Webhooks               | `new()`                       | `FaceitConfigWebhook`     | Settings related to Webhooks |
| Faceit:Webhooks:LogHooks      | `true`                        | `bool`                    | Whether to log all webhook events |
| Faceit:Chat                   | `new()`                       | `FaceitConfigChat`        | Settings related to FaceIT Chats |
| Faceit:Chat:LogLevel[^l]      | `LogLevel.Information`        | `LogLevel`		        | The default level to log everything at |
| Faceit:Chat:Url               | `"wss://chat-server.faceit.com/websocket"` | `string`     | URL of the websocket server for FaceIT chats |
| Faceit:Chat:Protocl           | `"xmpp"`                      | `string`                  | URL of the websocket server for FaceIT chats |
| Faceit:Chat:AppVersion        | `"2ebc5d5"`                   | `string`                  | The app version slug to use for the ws client |
| Faceit:Chat:FactionLeft       | `"faction1"`                  | `string`                  | The team key for the left-side team in matches |
| Faceit:Chat:FactionRight      | `"faction2"`                  | `string`                  | The team key for the right-side team |
| Faceit:Chat:KeepAlive[^k]     | `35` seconds                  | `double`                  | The number of seconds to keep the socket alive |
| Faceit:Chat:Reconnect         | `35` seconds                  | `double`                  | The number of seconds to wait between reconnects |
| Faceit:Chat:ReconnectError    | `35` seconds                  | `double`                  | The number of seconds to wait to reconnect after an error occurred |
| Faceit:Chat:PingInterval      | `30` seconds                  | `double`                  | The number of seconds to wait between server pings |
| Faceit:Chat:RequestTimeout    | `3` seconds                   | `double`                  | The number of seconds to wait before deeming a request timed out |
| Faceit:Chat:Encoding[^e]      | `Encoding.UTF8`               | `Encoding`                | The encoding to use for socket transmissions of binary data |

[^f]: Factories are resolved via settings when it's first configured.
[^k]: `Faceit:Chat:KeepAlive` needs to be greater than the `Faceit:Chat:PingInterval` (by a couple of seconds) otherwise your client will constantly reconnect before it can ping the server.
[^e]: `Faceit:Chat:Encoding` cannot be set via configuration objects and needs to be set manually via code. 
[^l]: This is an instance of `Microsoft.Extensions.Logging.LogLevel` and can be overridden via the underlying logging framework.

## Creating a config
You can create the `FaceitConfig` via one of two constructors: defaults or config based.

**Defaults**: To create an instance of `FaceitConfig` with the default values, you can just create a new instance with the parameterless constructor.
However, you will need to ensure you create the `Token` and `UserAgent` factories, otherwise the internal API and chats will throw errors whenever they are used.

Here is an example:
```csharp
using FaceitSharp.Core;

var apiToken = "<your_bearer_token>";
var userAgent = "<your_user_agent>"; 
var config = new FaceitConfig 
{
  Internal = new() 
  {
    Token = () => Task.FromResult(apiToken),
    UserAgent = () => Task.FromResult(userAgent)
  }
};
```

This will use the default options for everything that you don't explicitly set. You can refer to the table above for the defaults.

**Configuration Based**: To create an instance of `FaceitConfig` from an `IConfiguration` instance, 
you can just pass the section representing the FaceIT config to the other constructor.

```csharp
using FaceitSharp.Core;

IConfiguration config; //Get this from wherever you want

var section = config.GetSection("Faceit");
var config = new FaceitConfig(section);
```

Any proprety not set in the `IConfiguration` will use the defaults listed in the table above. 

## Getting Faceit Tokens
There are a few different types of FaceIT Tokens:

* Developer Tokens 
    - These are granted via the [developer settings page](https://developers.faceit.com) 
    - These are not currently used within FaceitSharp as they only work for the public API
* Internal Tokens 
    - These are internal tokens used on [faceit.com](https://faceit.com) 
    - You can find them by watching the network traffic on FaceIT and looking for the result from `https://www.faceit.com/api/auth-session/v1/token` 
    - This is the token you should use for FaceitSharp as it hooks into the interal API
* OAuth Tokens
    - These are granted from the OAuth protocol available on FaceIT. 
    - These are relatively new (as of 2024)
    - These are not currently used in FaceitSharp (but may be in the future)
    - This is used to resolve the `Internal Token` listed above