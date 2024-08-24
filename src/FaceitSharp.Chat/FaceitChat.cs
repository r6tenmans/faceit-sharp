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
    IFaceitInternalApiService _api,
    IResourceIdService _resourceId) : IFaceitChat
{

    public async ValueTask DisposeAsync()
    {
        SubscriptionsCleanup();
        await _chat.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().Wait();
        GC.SuppressFinalize(this);
    }
}
