using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.DTOs.Social;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class DirectMessageService : IDirectMessageService
{
    private readonly NeuroPathDbContext _db;

    public DirectMessageService(NeuroPathDbContext db) => _db = db;

    public async Task<List<DirectMessageDto>> GetConversationAsync(string userId, string otherUserId)
        => await _db.DirectMessages
            .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) || (m.SenderId == otherUserId && m.ReceiverId == userId))
            .Include(m => m.Sender)
            .OrderBy(m => m.SentAt)
            .Select(m => new DirectMessageDto
            {
                Id = m.Id, SenderId = m.SenderId, SenderUserName = m.Sender.UserName!,
                ReceiverId = m.ReceiverId, Content = m.Content, SentAt = m.SentAt, IsRead = m.IsRead
            }).ToListAsync();

    public async Task<List<DirectMessageDto>> GetRecentConversationsAsync(string userId)
    {
        var recentMessages = await _db.DirectMessages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .Include(m => m.Sender).Include(m => m.Receiver)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

        return recentMessages
            .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
            .Select(g => g.First())
            .Select(m => new DirectMessageDto
            {
                Id = m.Id, SenderId = m.SenderId, SenderUserName = m.Sender.UserName!,
                ReceiverId = m.ReceiverId, Content = m.Content, SentAt = m.SentAt, IsRead = m.IsRead
            }).ToList();
    }

    public async Task<DirectMessageDto> SendAsync(SendMessageDto dto, string senderId)
    {
        var m = new DirectMessage { SenderId = senderId, ReceiverId = dto.ReceiverId, Content = dto.Content };
        _db.DirectMessages.Add(m);
        await _db.SaveChangesAsync();
        var sender = await _db.Users.FindAsync(senderId);
        return new DirectMessageDto { Id = m.Id, SenderId = m.SenderId, SenderUserName = sender?.UserName ?? senderId, ReceiverId = m.ReceiverId, Content = m.Content, SentAt = m.SentAt, IsRead = m.IsRead };
    }

    public async Task<bool> MarkReadAsync(int messageId, string userId)
    {
        var m = await _db.DirectMessages.FirstOrDefaultAsync(x => x.Id == messageId && x.ReceiverId == userId);
        if (m == null) return false;
        m.IsRead = true; m.ReadAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }
}
