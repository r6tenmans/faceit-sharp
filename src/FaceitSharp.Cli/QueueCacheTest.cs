namespace FaceitSharp.Cli;

internal class QueueCacheTest(
    ILogger<QueueCacheTest> _logger) : ITest
{
    public async Task Basic()
    {
        int count = 0;
        var cache = new QueueCacheItem<int>(async () =>
        {
            _logger.LogWarning("Test Event Triggered for {count}", count);
            await Task.Delay(3000);
            count++;
            _logger.LogWarning("Test Event Resolved for {count}", count);
            return count;
        });

        var first = await cache.Get();
        _logger.LogInformation("First: {first}", first);
        var second = await cache.Get();
        _logger.LogInformation("Second: {second}", second);
        var third = await cache.Get();
        _logger.LogInformation("Third: {third}", third);
        cache.Bust();
        var forth = await cache.Get();
        _logger.LogInformation("Forth: {forth}", forth);
    }

    public async Task Queue()
    {
        int count = 0;
        var cache = new QueueCacheItem<int>(async () =>
        {
            _logger.LogWarning("Test Event Triggered for {count}", count);
            await Task.Delay(3000);
            count++;
            _logger.LogWarning("Test Event Resolved for {count}", count);
            return count;
        });

        var firstSet = await Task.WhenAll(cache.Get(), cache.Get(), cache.Get(), cache.Get());
        _logger.LogInformation("First Set: {firstSet}", string.Join(", ", firstSet));

        cache.Bust();
        var secondSet = await Task.WhenAll(cache.Get(), cache.Get(), cache.Get(), cache.Get());
        _logger.LogInformation("Second Set: {firstSet}", string.Join(", ", secondSet));
    }


    public async Task Run()
    {
        _logger.LogInformation("STARTING BASIC TEST");
        await Basic();
        _logger.LogInformation("BASIC TEST COMPLETE - STARTING QUEUE TEST");
        await Queue();
        _logger.LogInformation("QUEUE TEST COMPLETE");
    }
}
