using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class NotificationService : INotificationService
{
    private readonly NeuroPathDbContext _db;

    public NotificationService(NeuroPathDbContext db) => _db = db;

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
        => await _db.Notifications.Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto
            {
                Id = n.Id, Title = n.Title, Message = n.Message, Type = n.Type.ToString(),
                IsRead = n.IsRead, CreatedAt = n.CreatedAt, ReferenceId = n.ReferenceId, ReferenceType = n.ReferenceType
            }).ToListAsync();

    public async Task<bool> MarkReadAsync(int id, string userId)
    {
        var n = await _db.Notifications.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (n == null) return false;
        n.IsRead = true; n.ReadAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAllReadAsync(string userId)
    {
        var notifications = await _db.Notifications.Where(x => x.UserId == userId && !x.IsRead).ToListAsync();
        foreach (var n in notifications) { n.IsRead = true; n.ReadAt = DateTime.UtcNow; }
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var n = await _db.Notifications.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (n == null) return false;
        _db.Notifications.Remove(n);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task CreateAsync(string userId, string title, string message, string type, int? referenceId = null, string? referenceType = null)
    {
        if (!Enum.TryParse<NotificationType>(type, out var notifType)) notifType = NotificationType.AchievementUnlocked;
        _db.Notifications.Add(new Notification
        {
            UserId = userId, Title = title, Message = message, Type = notifType,
            ReferenceId = referenceId, ReferenceType = referenceType
        });
        await _db.SaveChangesAsync();
    }
}
