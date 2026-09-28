using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IActivityLogService
{
    Task<List<ActivityLogDto>> GetAllAsync(int page = 1, int pageSize = 50);
    Task LogAsync(string? userId, string description, string activityType, string? entityType = null, int? entityId = null, string? ipAddress = null);
}
