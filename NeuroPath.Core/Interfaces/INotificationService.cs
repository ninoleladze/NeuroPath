using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
    Task<bool> MarkReadAsync(int id, string userId);
    Task<bool> MarkAllReadAsync(string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task CreateAsync(string userId, string title, string message, string type, int? referenceId = null, string? referenceType = null);
}
