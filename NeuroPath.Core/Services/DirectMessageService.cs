using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Services;

public class DirectMessageService : IDirectMessageService
{
    private readonly INeuroPathDbContext _db;
    private readonly IFriendshipService _friendships;

    public DirectMessageService(INeuroPathDbContext db, IFriendshipService friendships)
    {
        _db = db;
        _friendships = friendships;
    }

    public async Task<List<DirectMessageDto>> GetConversationAsync(string userId, string otherUserId, int page = 1, int pageSize = 50)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        var messages = await _db.DirectMessages.AsNoTracking()
            .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) || (m.SenderId == otherUserId && m.ReceiverId == userId))
            .OrderByDescending(m => m.SentAt).ThenByDescending(m => m.Id)
            .Skip(skip).Take(take)
            .Select(m => new DirectMessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderUserName = m.Sender.UserName!,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            })
            .ToListAsync();
        messages.Reverse();
        return messages;
    }

    public async Task<List<DirectMessageDto>> GetRecentConversationsAsync(string userId, int limit = 20)
    {
        var latestIds = await _db.DirectMessages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
            .Select(g => g.Max(m => m.Id))
            .ToListAsync();

        return await _db.DirectMessages.AsNoTracking()
            .Where(m => latestIds.Contains(m.Id))
            .OrderByDescending(m => m.SentAt)
            .Take(Math.Clamp(limit, 1, Paging.MaxPageSize))
            .Select(m => new DirectMessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderUserName = m.Sender.UserName!,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            })
            .ToListAsync();
    }

    public async Task<DirectMessageDto> SendAsync(SendMessageDto dto, string senderId)
    {
        if (dto.ReceiverId == senderId)
            throw new BusinessRuleException("You cannot send a message to yourself.");
        if (!await _db.Users.AnyAsync(u => u.Id == dto.ReceiverId && u.IsActive && !u.IsBlocked))
            throw new NotFoundException("Recipient not found.");
        if (!await _friendships.AreFriendsAsync(senderId, dto.ReceiverId))
            throw new ForbiddenException("You can only message your friends.");

        var message = new DirectMessage { SenderId = senderId, ReceiverId = dto.ReceiverId, Content = dto.Content.Trim() };
        var senderName = await _db.Users.Where(u => u.Id == senderId).Select(u => u.UserName).FirstOrDefaultAsync() ?? string.Empty;

        _db.DirectMessages.Add(message);
        _db.Notifications.Add(new Notification
        {
            UserId = dto.ReceiverId,
            Title = "New message",
            Message = $"{senderName} sent you a message.",
            Type = NotificationType.MessageReceived,
            ReferenceType = nameof(DirectMessage)
        });
        await _db.SaveChangesAsync();

        return new DirectMessageDto
        {
            Id = message.Id,
            SenderId = senderId,
            SenderUserName = senderName,
            ReceiverId = message.ReceiverId,
            Content = message.Content,
            SentAt = message.SentAt,
            IsRead = false
        };
    }

    public async Task<bool> MarkReadAsync(int messageId, string userId)
        => await _db.DirectMessages
            .Where(m => m.Id == messageId && m.ReceiverId == userId && !m.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.IsRead, true).SetProperty(m => m.ReadAt, DateTime.UtcNow)) > 0;
}
