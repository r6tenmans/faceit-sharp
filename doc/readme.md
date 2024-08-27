# FaceitSharp
Documentation for interfacing with FaceIT using FaceitSharp.
This library is under active development and is subject to change with little to no notice. 

## Introduction
This library is aimed at automating a lot of the admin functions in FaceIT hubs.
This includes, but is not limited to, queue/permnantly banning players from hubs, administrating tickets and matches, and creating FaceIT chat bots.

The developers of this library work for [ChampionsForge](https://championsforge.pro), the largest Tom Clancy's Rainbow Six Siege organizer [on FaceIT](http://fce.gg/ChampionsForge). 
So a lot of this library is geared towards their use case. 
They also have special permissions when it comes to automating some actions as they have a lot of players to deal with.

## Installation 
You can install this project from [NuGet](https://www.nuget.org/packages/FaceitSharp).

```bash
PM> Install-Package FaceitSharp
```

## Usage
There are 3 primary parts of this library (for right now); the internal API, webhooks, and chat rooms.
These are all accessible via the `IFaceitApi` interface. This is a transient service and shouldn't be cached. 

You can create a single instance of the `IFaceitApi` using the following method (however it's not advised for larger applications).
First you need to generate the configuration to use for the application. Most of it has defaults that you don't really need to mess with. 
They only things you _have to_ set are your API token and the user-agent for your application. 

These are setup as factories so you can refresh them at your leasure and FaceitSharp will observe these changes.
These factories are called on every API request, so avoid long-running requests and cache where possible.
```csharp
using FaceitSharp;
using FaceitSharp.Core;

var apiToken = "<your_bearer_token>"; //Read the configuration documentation (linked below) for how to get this
var userAgent = "<your_user_agent>"; //This is purely to identify your application on FaceIT's side - Don't spoof it, you will be banned for that.

var config = new FaceitConfig
{
  Internal = new()
  {
    Token = () => Task.FromResult(apiToken),
    UserAgent = () => Task.FromResult(userAgent)
  }
};

//Pass the configuration to the api
IFaceitApi api = FaceitApi.Create(config);

//Now you can use the `api` as you want
```

Here is the perferred method for registering and using the API. This utilizes the standard dependency injection scheme from [Microsoft](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/):
```csharp
using FaceitSharp;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services; //Get this from wherever your service collection lives

//Add FaceitSharp to your services (without any webhook handlers).
//It fetches the api token and user-agent from your application config
services.AddFaceit(c => c.WithConfig());
```

For the various configuration options, you should check out [the configuration documentation](./config.md).