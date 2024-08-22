namespace FaceitSharp.Cli;

internal class TestRunner(
    IEnumerable<ITest> _tests,
    ILogger<TestRunner> _logger)
{
    public async Task<int> RunTests()
    {
        var errored = false;
        foreach (var test in _tests)
        {
            var name = test.GetType().Name;
            try
            {
                _logger.LogInformation("[TEST] {name} >> START", name);
                await test.Run();
                _logger.LogInformation("[TEST] {name} >> FINISHED", name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TEST] {name} >> ERROR", name);
                errored = true;
            }
        }

        return errored ? 1 : 0;
    }
}
