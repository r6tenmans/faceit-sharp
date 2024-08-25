using FaceitSharp.Chat.Network;

namespace FaceitSharp.Chat;

public interface IFaceitChat : 
    IDisposable, IAsyncDisposable,
    IFaceitChatAuth, IFaceitChatSubscriptions
{

}

internal partial class FaceitChat(
    ILogger<FaceitChat> _logger,
    IChatSocket _chat,
    IFaceitConfig _config,
    IFaceitChatCacheService _api,
    IResourceIdService _resourceId) : IFaceitChat
{
    private readonly List<IDisposable> _disposables = [];

    public async ValueTask DisposeAsync()
    {
        SubscriptionsCleanup();
        foreach (var disposable in _disposables)
            disposable.Dispose();
        _disposables.Clear();

        await _chat.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().Wait();
        GC.SuppressFinalize(this);
    }
}
