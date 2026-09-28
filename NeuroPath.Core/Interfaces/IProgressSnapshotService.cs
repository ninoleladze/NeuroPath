using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Core.Models.DTOs.Progress;

namespace NeuroPath.Core.Interfaces;

public interface IProgressSnapshotService
{
    Task<List<ProgressSnapshotDto>> GetSnapshotsAsync(string userId);
    Task<ProgressSummaryDto> GetSummaryAsync(string userId);
    Task<List<BrainAreaBreakdownDto>> GetBrainAreaBreakdownAsync(string userId);
    Task TakeSnapshotAsync(string userId);
}
