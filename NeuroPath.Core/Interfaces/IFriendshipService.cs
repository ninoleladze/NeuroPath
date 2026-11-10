using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Interfaces;

public interface IFriendshipService
{
    Task<List<FriendshipDto>> GetFriendsAsync(string userId);
    Task<bool> SendRequestAsync(SendFriendRequestDto dto, string senderId);
    Task<List<FriendRequestDto>> GetPendingRequestsAsync(string userId);
    Task<bool> AcceptRequestAsync(int requestId, string userId);
    Task<bool> RejectRequestAsync(int requestId, string userId);
    Task<bool> RemoveFriendAsync(string userId, string friendId);
    Task<bool> AreFriendsAsync(string userId, string otherUserId);
}
