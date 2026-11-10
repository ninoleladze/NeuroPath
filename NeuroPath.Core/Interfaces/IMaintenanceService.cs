namespace NeuroPath.Core.Interfaces;

public interface IMaintenanceService
{
    Task<int> CloseFinishedChallengesAsync(CancellationToken cancellationToken = default);
    Task RefreshLeaderboardsAsync(CancellationToken cancellationToken = default);
    Task<int> TakeDueSnapshotsAsync(CancellationToken cancellationToken = default);
    Task<int> PurgeExpiredTokensAsync(CancellationToken cancellationToken = default);
    Task<int> ResetBrokenStreaksAsync(CancellationToken cancellationToken = default);
}
