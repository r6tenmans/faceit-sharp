namespace FaceitSharp.Chat.Rooms;

public interface IChatRoomMatch : IChatRoom
{
    FaceitMatch Match { get; }
}

public class ChatRoomMatch(
    IFaceitChat _chat,
    IChatSocket _socket,
    IResourceIdService _resourceId,
    IFaceitInternalApiService _api,
    FaceitMatch _match) : ChatRoom(
        _chat, _socket, _resourceId, 
        new JID("conference.faceit.com", "match-" + _match.Id)), IChatRoomMatch
{
    public FaceitMatch Match { get; private set; } = _match;

    public override async Task Refresh()
    {
        Match = await _api.Matches.Get(Match.Id)
            ?? throw new Exception("Match not found");
    }
}
