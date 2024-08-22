using FaceitSharp;
using FaceitSharp.Cli;

return await new ServiceCollection()
    .AddSerilog()
    .AddConfig(c => c.AddFile("appsettings.json"), out var _config)
    .AddFaceit(c => c
        .WithConfig()
        .WithWebhookHandler<WebhookTestHandler>())
    .AddTransient<TestRunner>()

    .AddTransient<ITest, StandardTest>()
    .AddTransient<ITest, WebhookTest>()

    .BuildServiceProvider()
    .GetRequiredService<TestRunner>()
    .RunTests();
