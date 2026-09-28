using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Interfaces;

public interface IDirectMessageService
{
    Task<List<DirectMessageDto>> GetConversationAsync(string userId, string otherUserId);
    Task<List<DirectMessageDto>> GetRecentConversationsAsync(string userId);
    Task<DirectMessageDto> SendAsync(SendMessageDto dto, string senderId);
    Task<bool> MarkReadAsync(int messageId, string userId);
}
