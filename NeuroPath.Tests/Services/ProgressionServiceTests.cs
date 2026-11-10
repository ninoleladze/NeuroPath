using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Services;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class ProgressionServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task AwardPoints_UpdatesTotalAndBalanceAndWritesLedger()
    {
        await TestData.AddUserAsync(_database.Context, "player", balance: 5, totalPoints: 5);
        var service = new ProgressionService(_database.Context);

        await service.AwardPointsAsync("player", 20, PointSource.Exercise, 7);
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        var user = await check.Users.SingleAsync(u => u.Id == "player");
        var ledger = await check.PointTransactions.SingleAsync();
        Assert.Equal(25, user.TotalPoints);
        Assert.Equal(25, user.CurrentBalance);
        Assert.Equal(20, ledger.Amount);
        Assert.Equal(PointSource.Exercise, ledger.Source);
        Assert.Equal(7, ledger.ReferenceId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task AwardPoints_NonPositiveAmount_DoesNothing(int amount)
    {
        await TestData.AddUserAsync(_database.Context, "player", balance: 5, totalPoints: 5);

        await new ProgressionService(_database.Context).AwardPointsAsync("player", amount, PointSource.Quiz);
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        Assert.Equal(5, (await check.Users.SingleAsync()).TotalPoints);
        Assert.Equal(0, await check.PointTransactions.CountAsync());
    }

    [Fact]
    public async Task RegisterActivity_ConsecutiveDay_IncrementsStreak()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", current: 3, lastActivity: DateTime.UtcNow.AddDays(-1));

        await new ProgressionService(_database.Context).RegisterActivityAsync("player");
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        var streak = await check.Streaks.SingleAsync();
        Assert.Equal(4, streak.CurrentStreak);
        Assert.Equal(4, streak.LongestStreak);
    }

    [Fact]
    public async Task RegisterActivity_SameDay_DoesNotChangeStreak()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", current: 3, lastActivity: DateTime.UtcNow);

        await new ProgressionService(_database.Context).RegisterActivityAsync("player");
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        Assert.Equal(3, (await check.Streaks.SingleAsync()).CurrentStreak);
    }

    [Fact]
    public async Task RegisterActivity_MissedDayWithoutFreeze_ResetsStreak()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", current: 5, lastActivity: DateTime.UtcNow.AddDays(-2));

        await new ProgressionService(_database.Context).RegisterActivityAsync("player");
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        var streak = await check.Streaks.SingleAsync();
        Assert.Equal(1, streak.CurrentStreak);
        Assert.Equal(5, streak.LongestStreak);
    }

    [Fact]
    public async Task RegisterActivity_MissedDayWithFreeze_KeepsStreakAndUsesFreeze()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", current: 5, lastActivity: DateTime.UtcNow.AddDays(-2), freezes: 1);

        await new ProgressionService(_database.Context).RegisterActivityAsync("player");
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        var streak = await check.Streaks.SingleAsync();
        Assert.Equal(6, streak.CurrentStreak);
        Assert.Equal(0, streak.StreakFreezeCount);
    }

    [Fact]
    public async Task RegisterActivity_FirstActivityAfterRegistration_StartsStreakAtOne()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", current: 0, lastActivity: DateTime.UtcNow);

        await new ProgressionService(_database.Context).RegisterActivityAsync("player");
        await _database.Context.SaveChangesAsync();

        await using var check = _database.NewContext();
        Assert.Equal(1, (await check.Streaks.SingleAsync()).CurrentStreak);
    }
}
