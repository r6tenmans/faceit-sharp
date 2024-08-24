namespace FaceitSharp.Chat.XMPP;

public interface IResourceIdService
{
    long GetSeed();

    long Next();

    string ResourceId(string app = "new-frontend");
}

public class ResourceIdService(
    IFaceitConfig _config) : IResourceIdService
{
    public Random Random { get; private set; } = new();

    public long? Seed { get; private set; }

    public int Counter { get; private set; } = 0;

    public long GetSeed()
    {
        if (Seed is not null) return Seed.Value;

        Seed = Random.NextInt64((long)1e15);
        return Seed.Value;
    }

    public long Next()
    {
        return GetSeed() + Counter++;
    }

    public string ResourceId(string app = "new-frontend")
    {
        return $"{_config.Chat.AppVersion}_{app}_{GetSeed()}";
    }
}
