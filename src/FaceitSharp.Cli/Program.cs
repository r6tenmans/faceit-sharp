using FaceitSharp;
using FaceitSharp.Cli;

var config = new FaceitConfig
{
    Internal = new()
    {
        Token = () => Task.FromResult("Your FaceIT Token"),
        UserAgent = () => Task.FromResult("Your User Agent")
    }
};

var api = FaceitApi.Create(config);

return await new ServiceCollection()
    .AddSerilog()
    .AddConfig(c => c.AddFile("appsettings.json"), out var _config)
    .AddFaceit(c => c
        .WithConfig()
        .WithWebhookHandler<WebhookTestHandler>())
    .AddTransient<TestRunner>()

    //.AddTransient<ITest, QueueCacheTest>()
    //.AddTransient<ITest, StandardTest>()
    //.AddTransient<ITest, WebhookTest>()
    .AddTransient<ITest, ChatTest>()

    .BuildServiceProvider()
    .GetRequiredService<TestRunner>()
    .RunTests();
