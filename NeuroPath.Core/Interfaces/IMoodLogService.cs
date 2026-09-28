using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IMoodLogService
{
    Task<List<MoodLogDto>> GetUserLogsAsync(string userId);
    Task<MoodLogDto> CreateAsync(CreateMoodLogDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<object> GetStatsAsync(string userId);
}
