using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IWeeklyChallengeService
{
    Task<List<WeeklyChallengeDto>> GetAllAsync();
    Task<WeeklyChallengeDto?> GetByIdAsync(int id);
    Task<WeeklyChallengeDto> CreateAsync(CreateWeeklyChallengeDto dto);
    Task<WeeklyChallengeDto?> UpdateAsync(int id, CreateWeeklyChallengeDto dto);
    Task<bool> SetActiveAsync(int id, bool isActive);
    Task<bool> JoinAsync(int challengeId, string userId);
    Task<bool> JoinAsTeamAsync(int challengeId, int teamId, string requestingUserId);
    Task<List<ChallengeLeaderboardEntryDto>> GetLeaderboardAsync(int challengeId);
}
