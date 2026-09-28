using NeuroPath.Core.Models.DTOs.Training;

namespace NeuroPath.Core.Interfaces;

public interface IDailySessionService
{
    Task<List<DailySessionDto>> GetUserSessionsAsync(string userId);
    Task<DailySessionDto> StartSessionAsync(CreateDailySessionDto dto, string userId);
    Task<DailySessionDto?> CompleteSessionAsync(int sessionId, string userId);
    Task<bool> AddExerciseLogAsync(int sessionId, CreateUserExerciseLogDto dto, string userId);
    Task<List<UserExerciseLogDto>> GetUserLogsAsync(string userId);
    Task<object> GetStatsAsync(string userId);
}
