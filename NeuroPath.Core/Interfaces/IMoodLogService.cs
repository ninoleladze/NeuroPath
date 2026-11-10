using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IMoodLogService
{
    Task<List<MoodLogDto>> GetUserLogsAsync(string userId, int page = 1, int pageSize = 30);
    Task<MoodLogDto> CreateAsync(CreateMoodLogDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<MoodStatsDto> GetStatsAsync(string userId);
}
