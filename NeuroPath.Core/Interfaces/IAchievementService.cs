using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Interfaces;

public interface IAchievementService
{
    Task<List<AchievementDto>> GetAllAsync(bool includeSecret = false);
    Task<List<UserAchievementDto>> GetUserAchievementsAsync(string userId);
    Task<AchievementDto> CreateAsync(CreateAchievementDto dto);
    Task<AchievementDto?> UpdateAsync(int id, CreateAchievementDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<UserAchievementDto>> CheckAndAwardAsync(string userId);
}
