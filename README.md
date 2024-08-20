# faceit-sharp
C# API for interacting with FaceIT and it's chat rooms

## Installation
You can install the package using your favourite NuGet method
```bash	
PM> Install-Package FaceitSharp
```

## Usage
There are two different ways to use this library. 
There is a preferred method and a basic method. 
Both are outlined below.

### Basic Usage
This method isn't suggested for large applications, but it's a good way to get started and to play around.
```csharp
using FaceitSharp;

var config = new StaticFaceitConfig("your_api_key", "your_user_agent");
IFaceitApi api = FaceitApi.Create(config);

var yourAccount = await api.Internal.Users.Me();
var someoneElse = await api.Internal.Users.ByUsername("some_username");

var match = await api.Internal.Matches.Get("some_match_id");
```

### How you should do it
You should attach FaceitSharp to your Dependency Injection container,
with the appropriate configuration in your appsettings.json file 
(or your preferred method of `IConfiguration` handling).
```csharp
using FaceitSharp;

//Get this however you want:
IServiceCollection services;

services.AddFaceit(c => 
	c.WithConfig() //This will tell the library to look for your IConfiguration object
	 .WithWebhookHandler<SomeWebhookHandler>()); //This will tell the library to use your custom WebhookHandler

...

//Then you can use it like so:

public class SomeService(IFaceitApi _api)
{
	public async Task DoSomeStuff()
	{
		var yourAccount = await _api.Internal.Users.Me();
		var someoneElse = await _api.Internal.Users.ByUsername("some_username");

		var match = await _api.Internal.Matches.Get("some_match_id");
	}
}

...

//You can create custom webhook handlers like so:
using FaceitSharp.Webhooks;

public class SomeWebhookHandler(
	IFaceitApi _api) : FaceitWebhookEventHandler
{
	public override async Task HubUserAdded(FaceitWebhookDetails webhook, EventHubUserAdded user)
	{
		var profile = await _api.Internal.Users.ById(user.UserId);
		var hub = await _api.Internal.Hubs.Get(user.Id);

		//Now you have the user's profile and the hub they joined
	}
}
```

## FaceIT documentation
You can find the FaceIT documentation [here](https://developers.faceit.com).
You can also find the FaceIT developers discord server on that page as well (click the question icon on the bottom left of the side-bar).

For questions about FaceitSharp specifically, you can contact us on discord [here](https://discord.gg/championsforge).

Please note, that this library is not affiliated with FaceIT and is mostly used internally by ChampionsForge.
We have special permissions for certain aspects of this library that might not apply to just anyone, so use at your own risk.
