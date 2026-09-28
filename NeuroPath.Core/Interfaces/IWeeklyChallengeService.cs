using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IWeeklyChallengeService
{
    Task<List<WeeklyChallengeDto>> GetAllAsync();
    Task<WeeklyChallengeDto?> GetByIdAsync(int id);
    Task<WeeklyChallengeDto> CreateAsync(CreateWeeklyChallengeDto dto);
    Task<WeeklyChallengeDto?> UpdateAsync(int id, CreateWeeklyChallengeDto dto);
    Task<bool> JoinAsync(int challengeId, string userId);
    Task<bool> JoinAsTeamAsync(int challengeId, int teamId);
    Task<bool> UpdateProgressAsync(int challengeId, string userId, int points, int exercises, decimal accuracy);
    Task<List<object>> GetLeaderboardAsync(int challengeId);
}
