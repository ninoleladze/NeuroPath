using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Services;

public class FriendshipService : IFriendshipService
{
    private readonly INeuroPathDbContext _db;

    public FriendshipService(INeuroPathDbContext db) => _db = db;

    public async Task<List<FriendshipDto>> GetFriendsAsync(string userId)
    {
        var initiated = await _db.Friendships.AsNoTracking()
            .Where(f => f.UserId == userId)
            .Select(f => new FriendshipDto { FriendId = f.FriendId, FriendUserName = f.Friend.UserName!, FriendAvatarUrl = f.Friend.AvatarUrl, CreatedAt = f.CreatedAt })
            .ToListAsync();
        var received = await _db.Friendships.AsNoTracking()
            .Where(f => f.FriendId == userId)
            .Select(f => new FriendshipDto { FriendId = f.UserId, FriendUserName = f.User.UserName!, FriendAvatarUrl = f.User.AvatarUrl, CreatedAt = f.CreatedAt })
            .ToListAsync();

        return initiated.Concat(received).OrderBy(f => f.FriendUserName).ToList();
    }

    public async Task<bool> SendRequestAsync(SendFriendRequestDto dto, string senderId)
    {
        if (dto.ReceiverId == senderId)
            throw new BusinessRuleException("You cannot send a friend request to yourself.");
        if (!await _db.Users.AnyAsync(u => u.Id == dto.ReceiverId && u.IsActive && !u.IsBlocked))
            throw new NotFoundException("User not found.");
        if (await AreFriendsAsync(senderId, dto.ReceiverId))
            throw new ConflictException("You are already friends.");

        var pending = await _db.FriendRequests.FirstOrDefaultAsync(r =>
            r.Status == FriendRequestStatus.Pending &&
            ((r.SenderId == senderId && r.ReceiverId == dto.ReceiverId) || (r.SenderId == dto.ReceiverId && r.ReceiverId == senderId)));

        if (pending != null)
        {
            if (pending.SenderId == senderId) return false;
            return await AcceptRequestAsync(pending.Id, senderId);
        }

        var senderName = await _db.Users.Where(u => u.Id == senderId).Select(u => u.UserName).FirstOrDefaultAsync() ?? string.Empty;
        var request = new FriendRequest { SenderId = senderId, ReceiverId = dto.ReceiverId, Message = dto.Message };
        _db.FriendRequests.Add(request);
        _db.Notifications.Add(new Notification
        {
            UserId = dto.ReceiverId,
            Title = "New friend request",
            Message = $"{senderName} wants to be your friend.",
            Type = NotificationType.FriendRequest,
            ReferenceType = nameof(FriendRequest)
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<FriendRequestDto>> GetPendingRequestsAsync(string userId)
        => await _db.FriendRequests.AsNoTracking()
            .Where(r => r.ReceiverId == userId && r.Status == FriendRequestStatus.Pending)
            .OrderByDescending(r => r.SentAt)
            .Select(r => new FriendRequestDto
            {
                Id = r.Id,
                SenderId = r.SenderId,
                SenderUserName = r.Sender.UserName!,
                SenderAvatarUrl = r.Sender.AvatarUrl,
                Status = r.Status,
                SentAt = r.SentAt,
                Message = r.Message
            })
            .ToListAsync();

    public async Task<bool> AcceptRequestAsync(int requestId, string userId)
    {
        var request = await _db.FriendRequests.FirstOrDefaultAsync(r => r.Id == requestId && r.ReceiverId == userId && r.Status == FriendRequestStatus.Pending);
        if (request == null) return false;

        request.Status = FriendRequestStatus.Accepted;
        request.RespondedAt = DateTime.UtcNow;
        if (!await AreFriendsAsync(request.SenderId, request.ReceiverId))
            _db.Friendships.Add(new Friendship { UserId = request.SenderId, FriendId = request.ReceiverId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectRequestAsync(int requestId, string userId)
        => await _db.FriendRequests
            .Where(r => r.Id == requestId && r.ReceiverId == userId && r.Status == FriendRequestStatus.Pending)
            .ExecuteUpdateAsync(s => s
                .SetProperty(r => r.Status, FriendRequestStatus.Rejected)
                .SetProperty(r => r.RespondedAt, DateTime.UtcNow)) > 0;

    public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        => await _db.Friendships
            .Where(f => (f.UserId == userId && f.FriendId == friendId) || (f.UserId == friendId && f.FriendId == userId))
            .ExecuteDeleteAsync() > 0;

    public Task<bool> AreFriendsAsync(string userId, string otherUserId)
        => _db.Friendships.AnyAsync(f => (f.UserId == userId && f.FriendId == otherUserId) || (f.UserId == otherUserId && f.FriendId == userId));
}
