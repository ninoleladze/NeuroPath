using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Interfaces;

public interface ILeaderboardService
{
    Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(string period = "weekly");
    Task RefreshLeaderboardAsync(string period);
}
