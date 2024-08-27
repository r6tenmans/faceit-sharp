# Handling Webhooks
Coming soon...

Leaving this here for now:
```csharp
using FaceitSharp;
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

..

using FaceitSharp;

//Get this however you want:
IServiceCollection services;

services.AddFaceit(c => 
  c.WithConfig() //This will tell the library to look for your IConfiguration object
   .WithWebhookHandler<SomeWebhookHandler>()); //This will tell the library to use your custom WebhookHandler
```