using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly INeuroPathDbContext _db;

    public ActivityLogService(INeuroPathDbContext db) => _db = db;

    public async Task<List<ActivityLogDto>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.ActivityLogs.AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .Skip(skip).Take(take)
            .Select(l => new ActivityLogDto
            {
                Id = l.Id,
                UserId = l.UserId,
                UserName = l.User != null ? l.User.UserName : null,
                ActivityType = l.ActivityType,
                Description = l.Description,
                EntityType = l.EntityType,
                EntityId = l.EntityId,
                IpAddress = l.IpAddress,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();
    }

    public async Task LogAsync(string? userId, ActivityType activityType, string description, string? entityType = null, int? entityId = null, string? ipAddress = null)
    {
        _db.ActivityLogs.Add(new ActivityLog
        {
            UserId = userId,
            ActivityType = activityType,
            Description = description,
            EntityType = entityType,
            EntityId = entityId,
            IpAddress = ipAddress
        });
        await _db.SaveChangesAsync();
    }
}
