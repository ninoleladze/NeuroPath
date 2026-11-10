using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Settings;

namespace NeuroPath.Core.Services;

public class MaintenanceService : IMaintenanceService
{
    private static readonly string[] LeaderboardPeriods = { "weekly", "monthly", "alltime" };

    private readonly INeuroPathDbContext _db;
    private readonly ILeaderboardService _leaderboard;
    private readonly IProgressSnapshotService _snapshots;
    private readonly BackgroundJobSettings _settings;

    public MaintenanceService(INeuroPathDbContext db, ILeaderboardService leaderboard, IProgressSnapshotService snapshots, IOptions<BackgroundJobSettings> settings)
    {
        _db = db;
        _leaderboard = leaderboard;
        _snapshots = snapshots;
        _settings = settings.Value;
    }

    public async Task<int> CloseFinishedChallengesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var failedUsers = await _db.UserChallenges
            .Where(uc => uc.Status != ChallengeStatus.Completed && uc.Status != ChallengeStatus.Failed && uc.WeeklyChallenge.EndDate < now)
            .ExecuteUpdateAsync(s => s.SetProperty(uc => uc.Status, ChallengeStatus.Failed), cancellationToken);

        var failedTeams = await _db.TeamChallengeEntries
            .Where(e => e.Status != ChallengeStatus.Completed && e.Status != ChallengeStatus.Failed && e.WeeklyChallenge.EndDate < now)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.Status, ChallengeStatus.Failed), cancellationToken);

        var closed = await _db.WeeklyChallenges
            .Where(c => c.IsActive && c.EndDate < now)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsActive, false), cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return closed + failedUsers + failedTeams;
    }

    public async Task RefreshLeaderboardsAsync(CancellationToken cancellationToken = default)
    {
        foreach (var period in LeaderboardPeriods)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _leaderboard.RefreshLeaderboardAsync(period);
        }
    }

    public async Task<int> TakeDueSnapshotsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var dueBefore = now.AddDays(-_settings.SnapshotIntervalDays);
        var activeSince = now.AddDays(-_settings.ActiveUserWindowDays);

        var userIds = await _db.Users
            .Where(u => u.IsActive && !u.IsBlocked
                && u.Streak != null && u.Streak.LastActivityDate >= activeSince
                && !_db.ProgressSnapshots.Any(p => p.UserId == u.Id && p.SnapshotDate >= dueBefore))
            .OrderBy(u => u.Id)
            .Select(u => u.Id)
            .Take(_settings.SnapshotBatchSize)
            .ToListAsync(cancellationToken);

        foreach (var userId in userIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _snapshots.TakeSnapshotAsync(userId);
        }

        return userIds.Count;
    }

    public async Task<int> PurgeExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var retentionCutoff = now.AddDays(-_settings.RevokedTokenRetentionDays);

        var refresh = await _db.RefreshTokens
            .Where(t => t.ExpiresAt < now || (t.RevokedAt != null && t.RevokedAt < retentionCutoff))
            .ExecuteDeleteAsync(cancellationToken);

        var verification = await _db.EmailVerificationTokens
            .Where(t => t.ExpiresAt < retentionCutoff || (t.IsUsed && t.UsedAt < retentionCutoff))
            .ExecuteDeleteAsync(cancellationToken);

        var reset = await _db.PasswordResetTokens
            .Where(t => t.ExpiresAt < retentionCutoff || (t.IsUsed && t.UsedAt < retentionCutoff))
            .ExecuteDeleteAsync(cancellationToken);

        return refresh + verification + reset;
    }
}
