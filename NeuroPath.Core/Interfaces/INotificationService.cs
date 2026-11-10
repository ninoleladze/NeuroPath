using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = 20);
    Task<int> GetUnreadCountAsync(string userId);
    Task<bool> MarkReadAsync(int id, string userId);
    Task<int> MarkAllReadAsync(string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task CreateAsync(string userId, string title, string message, NotificationType type, int? referenceId = null, string? referenceType = null);
}
