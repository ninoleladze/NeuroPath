using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Core.Services;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class AchievementServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    private AchievementService CreateService() => new(_database.Context, _database.Cache, new ProgressionService(_database.Context));

    private async Task<Achievement> AddAchievementAsync(int? pointsRequired, int bonus = 25, int? streakRequired = null)
    {
        var achievement = new Achievement { Name = $"A{Guid.NewGuid():N}"[..12], Description = "Test", PointsRequired = pointsRequired, StreakDaysRequired = streakRequired, BonusPoints = bonus };
        _database.Context.Achievements.Add(achievement);
        await _database.Context.SaveChangesAsync();
        return achievement;
    }

    [Fact]
    public async Task CheckAndAward_GrantsQualifiedAchievementsOnceWithBonusAndNotification()
    {
        await TestData.AddUserAsync(_database.Context, "player", balance: 500, totalPoints: 500);
        var reachable = await AddAchievementAsync(pointsRequired: 100, bonus: 25);
        await AddAchievementAsync(pointsRequired: 10_000);

        var first = await CreateService().CheckAndAwardAsync("player");
        var second = await CreateService().CheckAndAwardAsync("player");

        Assert.Equal(reachable.Id, Assert.Single(first).AchievementId);
        Assert.Empty(second);

        await using var check = _database.NewContext();
        var user = await check.Users.SingleAsync(u => u.Id == "player");
        Assert.Equal(525, user.TotalPoints);
        Assert.Equal(525, user.CurrentBalance);
        Assert.Equal(1, await check.UserAchievements.CountAsync(a => a.UserId == "player"));
        Assert.Equal(1, await check.Notifications.CountAsync(n => n.UserId == "player" && n.Type == NotificationType.AchievementUnlocked));
        Assert.Equal(1, await check.PointTransactions.CountAsync(t => t.UserId == "player" && t.Source == PointSource.Achievement));
    }

    [Fact]
    public async Task CheckAndAward_RequiresEveryConditionToBeMet()
    {
        await TestData.AddUserAsync(_database.Context, "player", totalPoints: 500);
        await TestData.AddStreakAsync(_database.Context, "player", current: 2, lastActivity: DateTime.UtcNow);
        await AddAchievementAsync(pointsRequired: 100, streakRequired: 7);

        var awarded = await CreateService().CheckAndAwardAsync("player");

        Assert.Empty(awarded);
    }

    [Fact]
    public async Task Delete_IsBlockedOnceSomeoneEarnedTheAchievement()
    {
        await TestData.AddUserAsync(_database.Context, "player", totalPoints: 500);
        var achievement = await AddAchievementAsync(pointsRequired: 1);
        await CreateService().CheckAndAwardAsync("player");

        await Assert.ThrowsAsync<ConflictException>(() => CreateService().DeleteAsync(achievement.Id));
    }

    [Fact]
    public async Task SecretAchievements_AreHiddenUnlessRequested()
    {
        var service = CreateService();
        await service.CreateAsync(new CreateAchievementDto { Name = "Visible", Description = "Shown" });
        await service.CreateAsync(new CreateAchievementDto { Name = "Hidden", Description = "Secret", IsSecret = true });

        Assert.DoesNotContain(await service.GetAllAsync(), a => a.Name == "Hidden");
        Assert.Contains(await service.GetAllAsync(includeSecret: true), a => a.Name == "Hidden");
    }
}
