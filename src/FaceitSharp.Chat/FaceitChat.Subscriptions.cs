using FaceitSharp.Chat.Rooms;

namespace FaceitSharp.Chat;

public interface IFaceitChatSubscriptions
{
    Task<IChatRoomMatch> GetMatch(string id);
}

internal partial class FaceitChat
{
    private readonly ConcurrentDictionary<string, IChatRoom> _chatSubscriptions = [];

    public async Task<IChatRoomMatch> GetMatch(string id)
    {
        if (_chatSubscriptions.TryGetValue(id, out var room))
        { 
            if (room is not IChatRoomMatch matchRoom)
                throw new Exception($"Room is invalid, did you mean I{room.GetType().Name}?");

            return matchRoom;
        }
        
        var match = await _api.Matches.Get(id)
            ?? throw new Exception("Match not found");

        var subscription = ChatSubscription.Create(SubscriptionType.Match, id, Current!.UserId, Jid!, false, false, 10);
        _ = await _chat.Send(subscription) 
            ?? throw new Exception("Failed to subscribe to presence: " + id);

        room = new ChatRoomMatch(this, _chat, _resourceId, _api, match);
        _chatSubscriptions.TryAdd(id, room);
        return (IChatRoomMatch)room;
    }

    //public async Task<IObservable<Stanza>> OnTeam(string matchId, string teamId)
    //    => await Subscribe(SubscriptionType.Team, $"{matchId}_{teamId}");

    public void SubscriptionsCleanup()
    {
        _chatSubscriptions.Clear();
    }

    public void SubscriptionsSetup()
    {

    }
}
