using NeuroPath.Core.Models.DTOs.Training;

namespace NeuroPath.Core.Interfaces;

public interface IDailySessionService
{
    Task<List<DailySessionDto>> GetUserSessionsAsync(string userId, int page = 1, int pageSize = 20);
    Task<DailySessionDto> StartSessionAsync(CreateDailySessionDto dto, string userId);
    Task<DailySessionDto?> CompleteSessionAsync(int sessionId, string userId);
    Task<ExerciseLogResultDto?> AddExerciseLogAsync(int sessionId, CreateUserExerciseLogDto dto, string userId);
    Task<List<UserExerciseLogDto>> GetUserLogsAsync(string userId, int page = 1, int pageSize = 20);
    Task<SessionStatsDto> GetStatsAsync(string userId);
}
