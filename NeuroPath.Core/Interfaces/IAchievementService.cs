using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Interfaces;

public interface IAchievementService
{
    Task<List<AchievementDto>> GetAllAsync();
    Task<List<UserAchievementDto>> GetUserAchievementsAsync(string userId);
    Task<AchievementDto> CreateAsync(AchievementDto dto);
    Task<AchievementDto?> UpdateAsync(int id, AchievementDto dto);
    Task<bool> DeleteAsync(int id);
    Task CheckAndAwardAsync(string userId);
}
