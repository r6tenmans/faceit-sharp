# Internal API
Coming soon...

Leaving this here for now:
```csharp
using FaceitSharp;
using FaceitSharp.Core;

var apiToken = "you_api_key";
var userAgent = "your_user_agent";

var config = new FaceitConfig()
{
  Internal = new() 
  {
    Token = () => Task.FromResult(apiToken),
    UserAgent = () => Task.FromResult(userAgent)
  }
};
IFaceitApi api = FaceitApi.Create(config);

var yourAccount = await api.Internal.Users.Me();
var someoneElse = await api.Internal.Users.ByUsername("some_username");

var match = await api.Internal.Matches.Get("some_match_id");
```