using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class NotificationService : INotificationService
{
    private readonly INeuroPathDbContext _db;

    public NotificationService(INeuroPathDbContext db) => _db = db;

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        var rows = await _db.Notifications.AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip(skip).Take(take)
            .ToListAsync();
        return rows.Select(n => new NotificationDto
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type.ToString(),
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
            ReferenceId = n.ReferenceId,
            ReferenceType = n.ReferenceType
        }).ToList();
    }

    public Task<int> GetUnreadCountAsync(string userId)
        => _db.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task<bool> MarkReadAsync(int id, string userId)
        => await _db.Notifications
            .Where(n => n.Id == id && n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true).SetProperty(n => n.ReadAt, DateTime.UtcNow)) > 0;

    public Task<int> MarkAllReadAsync(string userId)
        => _db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true).SetProperty(n => n.ReadAt, DateTime.UtcNow));

    public async Task<bool> DeleteAsync(int id, string userId)
        => await _db.Notifications.Where(n => n.Id == id && n.UserId == userId).ExecuteDeleteAsync() > 0;

    public async Task CreateAsync(string userId, string title, string message, NotificationType type, int? referenceId = null, string? referenceType = null)
    {
        _db.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            ReferenceId = referenceId,
            ReferenceType = referenceType
        });
        await _db.SaveChangesAsync();
    }
}
