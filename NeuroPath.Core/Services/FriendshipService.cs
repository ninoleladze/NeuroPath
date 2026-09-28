using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.DTOs.Social;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class FriendshipService : IFriendshipService
{
    private readonly NeuroPathDbContext _db;

    public FriendshipService(NeuroPathDbContext db) => _db = db;

    public async Task<List<FriendshipDto>> GetFriendsAsync(string userId)
    {
        var initiated = await _db.Friendships.Where(f => f.UserId == userId).Include(f => f.Friend)
            .Select(f => new FriendshipDto { FriendId = f.FriendId, FriendUserName = f.Friend.UserName!, FriendAvatarUrl = f.Friend.AvatarUrl, CreatedAt = f.CreatedAt })
            .ToListAsync();
        var received = await _db.Friendships.Where(f => f.FriendId == userId).Include(f => f.User)
            .Select(f => new FriendshipDto { FriendId = f.UserId, FriendUserName = f.User.UserName!, FriendAvatarUrl = f.User.AvatarUrl, CreatedAt = f.CreatedAt })
            .ToListAsync();
        return initiated.Concat(received).ToList();
    }

    public async Task<bool> SendRequestAsync(SendFriendRequestDto dto, string senderId)
    {
        if (await _db.FriendRequests.AnyAsync(f => f.SenderId == senderId && f.ReceiverId == dto.ReceiverId && f.Status == FriendRequestStatus.Pending))
            return false;
        _db.FriendRequests.Add(new FriendRequest { SenderId = senderId, ReceiverId = dto.ReceiverId, Message = dto.Message });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<FriendRequestDto>> GetPendingRequestsAsync(string userId)
        => await _db.FriendRequests.Where(f => f.ReceiverId == userId && f.Status == FriendRequestStatus.Pending)
            .Include(f => f.Sender)
            .Select(f => new FriendRequestDto { Id = f.Id, SenderId = f.SenderId, SenderUserName = f.Sender.UserName!, SenderAvatarUrl = f.Sender.AvatarUrl, Status = f.Status, SentAt = f.SentAt, Message = f.Message })
            .ToListAsync();

    public async Task<bool> AcceptRequestAsync(int requestId, string userId)
    {
        var req = await _db.FriendRequests.FirstOrDefaultAsync(f => f.Id == requestId && f.ReceiverId == userId && f.Status == FriendRequestStatus.Pending);
        if (req == null) return false;
        req.Status = FriendRequestStatus.Accepted; req.RespondedAt = DateTime.UtcNow;
        _db.Friendships.Add(new Friendship { UserId = req.SenderId, FriendId = req.ReceiverId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectRequestAsync(int requestId, string userId)
    {
        var req = await _db.FriendRequests.FirstOrDefaultAsync(f => f.Id == requestId && f.ReceiverId == userId && f.Status == FriendRequestStatus.Pending);
        if (req == null) return false;
        req.Status = FriendRequestStatus.Rejected; req.RespondedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFriendAsync(string userId, string friendId)
    {
        var f1 = await _db.Friendships.FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);
        var f2 = await _db.Friendships.FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId);
        if (f1 == null && f2 == null) return false;
        if (f1 != null) _db.Friendships.Remove(f1);
        if (f2 != null) _db.Friendships.Remove(f2);
        await _db.SaveChangesAsync();
        return true;
    }
}
