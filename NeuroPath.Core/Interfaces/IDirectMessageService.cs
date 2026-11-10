using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Interfaces;

public interface IDirectMessageService
{
    Task<List<DirectMessageDto>> GetConversationAsync(string userId, string otherUserId, int page = 1, int pageSize = 50);
    Task<List<DirectMessageDto>> GetRecentConversationsAsync(string userId, int limit = 20);
    Task<DirectMessageDto> SendAsync(SendMessageDto dto, string senderId);
    Task<bool> MarkReadAsync(int messageId, string userId);
}
