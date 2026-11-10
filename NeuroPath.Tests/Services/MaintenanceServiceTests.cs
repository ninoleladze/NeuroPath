using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Challenges;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Services;
using NeuroPath.Core.Settings;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class MaintenanceServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    private MaintenanceService CreateService(int streakResetAfterDays = 3, int revokedRetentionDays = 30)
        => new(_database.Context, null!, null!, Options.Create(new BackgroundJobSettings
        {
            StreakResetAfterDays = streakResetAfterDays,
            RevokedTokenRetentionDays = revokedRetentionDays
        }));

    [Fact]
    public async Task ResetBrokenStreaks_ResetsOnlyStreaksOlderThanTheCutoff()
    {
        await TestData.AddUserAsync(_database.Context, "stale");
        await TestData.AddUserAsync(_database.Context, "recent");
        await TestData.AddStreakAsync(_database.Context, "stale", current: 9, lastActivity: DateTime.UtcNow.AddDays(-5));
        await TestData.AddStreakAsync(_database.Context, "recent", current: 4, lastActivity: DateTime.UtcNow.AddDays(-1));

        var reset = await CreateService().ResetBrokenStreaksAsync();

        await using var check = _database.NewContext();
        Assert.Equal(1, reset);
        var stale = await check.Streaks.SingleAsync(s => s.UserId == "stale");
        Assert.Equal(0, stale.CurrentStreak);
        Assert.Equal(9, stale.LongestStreak);
        Assert.Equal(4, (await check.Streaks.SingleAsync(s => s.UserId == "recent")).CurrentStreak);
    }

    [Fact]
    public async Task ResetBrokenStreaks_NeverUsesACutoffShorterThanThreeDays()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", current: 3, lastActivity: DateTime.UtcNow.AddDays(-2));

        var reset = await CreateService(streakResetAfterDays: 1).ResetBrokenStreaksAsync();

        Assert.Equal(0, reset);
    }

    [Fact]
    public async Task PurgeExpiredTokens_KeepsActiveAndRecentlyRevokedTokens()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        var now = DateTime.UtcNow;
        _database.Context.RefreshTokens.AddRange(
            new RefreshToken { UserId = "player", TokenHash = "active", ExpiresAt = now.AddDays(5) },
            new RefreshToken { UserId = "player", TokenHash = "expired", ExpiresAt = now.AddMinutes(-1) },
            new RefreshToken { UserId = "player", TokenHash = "old-revoked", ExpiresAt = now.AddDays(5), RevokedAt = now.AddDays(-40) },
            new RefreshToken { UserId = "player", TokenHash = "new-revoked", ExpiresAt = now.AddDays(5), RevokedAt = now.AddDays(-1) });
        await _database.Context.SaveChangesAsync();

        var purged = await CreateService().PurgeExpiredTokensAsync();

        await using var check = _database.NewContext();
        Assert.Equal(2, purged);
        var remaining = await check.RefreshTokens.Select(t => t.TokenHash).OrderBy(h => h).ToListAsync();
        Assert.Equal(new[] { "active", "new-revoked" }, remaining);
    }

    [Fact]
    public async Task CloseFinishedChallenges_FailsUnfinishedParticipantsAndDeactivatesTheChallenge()
    {
        await TestData.AddUserAsync(_database.Context, "joined");
        await TestData.AddUserAsync(_database.Context, "winner");
        var now = DateTime.UtcNow;
        var finished = new WeeklyChallenge { Title = "Past", Description = "Past", StartDate = now.AddDays(-14), EndDate = now.AddDays(-7), TargetExerciseCount = 5, TargetPoints = 100 };
        var running = new WeeklyChallenge { Title = "Now", Description = "Now", StartDate = now.AddDays(-1), EndDate = now.AddDays(6), TargetExerciseCount = 5, TargetPoints = 100 };
        _database.Context.WeeklyChallenges.AddRange(finished, running);
        await _database.Context.SaveChangesAsync();
        _database.Context.UserChallenges.AddRange(
            new UserChallenge { UserId = "joined", WeeklyChallengeId = finished.Id, Status = ChallengeStatus.Joined },
            new UserChallenge { UserId = "winner", WeeklyChallengeId = finished.Id, Status = ChallengeStatus.Completed },
            new UserChallenge { UserId = "joined", WeeklyChallengeId = running.Id, Status = ChallengeStatus.Joined });
        await _database.Context.SaveChangesAsync();

        await CreateService().CloseFinishedChallengesAsync();

        await using var check = _database.NewContext();
        Assert.False((await check.WeeklyChallenges.SingleAsync(c => c.Id == finished.Id)).IsActive);
        Assert.True((await check.WeeklyChallenges.SingleAsync(c => c.Id == running.Id)).IsActive);
        Assert.Equal(ChallengeStatus.Failed, (await check.UserChallenges.SingleAsync(c => c.UserId == "joined" && c.WeeklyChallengeId == finished.Id)).Status);
        Assert.Equal(ChallengeStatus.Completed, (await check.UserChallenges.SingleAsync(c => c.UserId == "winner")).Status);
        Assert.Equal(ChallengeStatus.Joined, (await check.UserChallenges.SingleAsync(c => c.WeeklyChallengeId == running.Id)).Status);
    }
}
