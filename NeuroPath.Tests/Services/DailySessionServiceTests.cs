using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.DTOs.Training;
using NeuroPath.Core.Services;
using NeuroPath.Core.Settings;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class DailySessionServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    private DailySessionService CreateService(GamificationSettings? rules = null)
    {
        var progression = new ProgressionService(_database.Context);
        var achievements = new AchievementService(_database.Context, _database.Cache, progression);
        return new DailySessionService(_database.Context, progression, achievements, Options.Create(rules ?? new GamificationSettings()));
    }

    private static CreateUserExerciseLogDto Log(int exerciseId, int seconds = 600) => new()
    {
        ExerciseId = exerciseId,
        Score = 100,
        DurationSeconds = seconds,
        AccuracyPercent = 100,
        HintsUsed = 0
    };

    [Fact]
    public async Task AddExerciseLog_FasterThanMinimumDuration_IsRejected()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        var exercise = await TestData.AddExerciseAsync(_database.Context, durationMinutes: 10);
        var service = CreateService();
        var session = await service.StartSessionAsync(new CreateDailySessionDto { SessionDate = DateTime.UtcNow }, "player");

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.AddExerciseLogAsync(session.Id, Log(exercise.Id, seconds: 30), "player"));

        Assert.Equal(0, await _database.NewContext().UserExerciseLogs.CountAsync());
    }

    [Fact]
    public async Task AddExerciseLog_AfterMaxScoredRepeats_AwardsNoPoints()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        var exercise = await TestData.AddExerciseAsync(_database.Context, points: 100);
        var service = CreateService(new GamificationSettings { MaxScoredRepeatsPerExercisePerDay = 2, DailyExercisePointCap = 10_000 });
        var session = await service.StartSessionAsync(new CreateDailySessionDto { SessionDate = DateTime.UtcNow }, "player");

        var first = await service.AddExerciseLogAsync(session.Id, Log(exercise.Id), "player");
        var second = await service.AddExerciseLogAsync(session.Id, Log(exercise.Id), "player");
        var third = await service.AddExerciseLogAsync(session.Id, Log(exercise.Id), "player");

        Assert.Equal(100, first!.PointsAwarded);
        Assert.Equal(100, second!.PointsAwarded);
        Assert.Equal(0, third!.PointsAwarded);
        Assert.Equal(3, third.SessionTotalExercises);

        await using var check = _database.NewContext();
        Assert.Equal(200, (await check.Users.SingleAsync(u => u.Id == "player")).TotalPoints);
    }

    [Fact]
    public async Task AddExerciseLog_DailyPointCap_LimitsAwardedPoints()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        var first = await TestData.AddExerciseAsync(_database.Context, points: 100);
        var second = await TestData.AddExerciseAsync(_database.Context, points: 100);
        var service = CreateService(new GamificationSettings { MaxScoredRepeatsPerExercisePerDay = 5, DailyExercisePointCap = 150 });
        var session = await service.StartSessionAsync(new CreateDailySessionDto { SessionDate = DateTime.UtcNow }, "player");

        var a = await service.AddExerciseLogAsync(session.Id, Log(first.Id), "player");
        var b = await service.AddExerciseLogAsync(session.Id, Log(second.Id), "player");
        var c = await service.AddExerciseLogAsync(session.Id, Log(second.Id), "player");

        Assert.Equal(100, a!.PointsAwarded);
        Assert.Equal(50, b!.PointsAwarded);
        Assert.Equal(0, c!.PointsAwarded);
    }
}
