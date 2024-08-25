using FaceitSharp.Chat.Network;

namespace FaceitSharp.Chat;

public static class DiExtensions
{
    public static IServiceCollection AddFaceitChat(this IServiceCollection services)
    {
        return services
            .AddSingleton<IChatSocket, ChatSocket>()
            .AddSingleton<IFaceitChat, FaceitChat>()
            .AddSingleton<IResourceIdService, ResourceIdService>()
            .AddSingleton<IFaceitChatCacheService, FaceitChatCacheService>();
    }
}
