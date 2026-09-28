using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly NeuroPathDbContext _db;

    public ActivityLogService(NeuroPathDbContext db) => _db = db;

    public async Task<List<ActivityLogDto>> GetAllAsync(int page = 1, int pageSize = 50)
        => await _db.ActivityLogs.Include(l => l.User)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(l => new ActivityLogDto { Id = l.Id, UserId = l.UserId, UserName = l.User != null ? l.User.UserName : null, ActivityType = l.ActivityType, Description = l.Description, EntityType = l.EntityType, EntityId = l.EntityId, IpAddress = l.IpAddress, CreatedAt = l.CreatedAt })
            .ToListAsync();

    public async Task LogAsync(string? userId, string description, string activityType, string? entityType = null, int? entityId = null, string? ipAddress = null)
    {
        if (!Enum.TryParse<ActivityType>(activityType, out var type)) type = ActivityType.Login;
        _db.ActivityLogs.Add(new ActivityLog { UserId = userId, Description = description, ActivityType = type, EntityType = entityType, EntityId = entityId, IpAddress = ipAddress });
        await _db.SaveChangesAsync();
    }
}
