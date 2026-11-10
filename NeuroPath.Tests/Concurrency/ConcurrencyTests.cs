using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Quiz;
using NeuroPath.Core.Models.DTOs.Training;
using NeuroPath.Core.Services;
using NeuroPath.Core.Settings;
using NeuroPath.Data.Context;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Concurrency;

public class ConcurrencyTests
{
    private const int Parallelism = 4;

    private static IOptions<GamificationSettings> Rules { get; } = Options.Create(new GamificationSettings { MaxScoredRepeatsPerExercisePerDay = 100 });

    private static AchievementService Achievements(NeuroPathDbContext context, ProgressionService progression)
        => new(context, new CacheService(new MemoryCache(new MemoryCacheOptions())), progression);

    [Fact]
    public async Task ParallelQuizSubmissions_AwardFirstPassPointsOnce()
    {
        using var database = new SqliteFileDatabase();
        database.EnsureCreated();
        SubmitQuizDto dto;
        await using (var seed = database.NewContext())
        {
            await TestData.AddUserAsync(seed, "author");
            await TestData.AddUserAsync(seed, "player");
            var quiz = await TestData.AddPublishedQuizAsync(seed, "author");
            dto = new SubmitQuizDto
            {
                QuizId = quiz.Id,
                Answers = quiz.Questions.Select(q => new UserAnswerSubmission { QuestionId = q.Id, SelectedAnswerId = TestData.CorrectAnswerId(q) }).ToList()
            };
        }

        var results = await Task.WhenAll(Enumerable.Range(0, Parallelism).Select(_ => Task.Run(async () =>
        {
            await using var context = database.NewContext();
            var progression = new ProgressionService(context);
            var service = new QuizService(context, progression, Achievements(context, progression));
            try
            {
                return (await service.SubmitAsync(dto, "player")).PointsAwarded;
            }
            catch (ConflictException)
            {
                return -1;
            }
        })));

        await using var check = database.NewContext();
        Assert.Equal(1, results.Count(points => points > 0));
        Assert.Equal(1, await check.UserQuizAttempts.CountAsync(a => a.PointsAwarded > 0));
        Assert.Equal(23, (await check.Users.SingleAsync(u => u.Id == "player")).TotalPoints);
        Assert.Equal(23, await check.PointTransactions.Where(t => t.UserId == "player").SumAsync(t => t.Amount));
    }

    [Fact]
    public async Task ParallelAchievementChecks_AwardEachAchievementOnce()
    {
        using var database = new SqliteFileDatabase();
        database.EnsureCreated();
        await using (var seed = database.NewContext())
        {
            await TestData.AddUserAsync(seed, "player", balance: 50, totalPoints: 50);
            seed.Achievements.Add(new Achievement { Name = "Half century", Description = "50 points", PointsRequired = 50, BonusPoints = 5 });
            await seed.SaveChangesAsync();
        }

        await Task.WhenAll(Enumerable.Range(0, Parallelism).Select(_ => Task.Run(async () =>
        {
            await using var context = database.NewContext();
            await Achievements(context, new ProgressionService(context)).CheckAndAwardAsync("player");
        })));

        await using var check = database.NewContext();
        Assert.Equal(1, await check.UserAchievements.CountAsync());
        Assert.Equal(1, await check.Notifications.CountAsync());
        Assert.Equal(55, (await check.Users.SingleAsync(u => u.Id == "player")).TotalPoints);
    }

    [Fact]
    public async Task ParallelRefreshesWithOneToken_AllSucceedWithoutRevokingTheSession()
    {
        using var host = await IdentityTestHost.CreateAsync();
        const string email = "tabs@neuropath.test";
        await host.RegisterVerifiedUserAsync(email);
        var login = await host.LoginAsync(email);
        string stampBefore;
        await using (var db = host.NewContext())
            stampBefore = (await db.Users.SingleAsync(u => u.Email == email)).SecurityStamp!;

        var results = await Task.WhenAll(Enumerable.Range(0, Parallelism).Select(_ =>
            Task.Run(() => host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null)))));

        Assert.Equal(Parallelism, results.Select(r => r.RefreshToken).Distinct().Count());
        await using var check = host.NewContext();
        Assert.Equal(stampBefore, (await check.Users.SingleAsync(u => u.Email == email)).SecurityStamp);
        foreach (var result in results)
            Assert.False(string.IsNullOrEmpty((await host.RunAsync(auth => auth.RefreshAsync(result.RefreshToken!, null))).AccessToken));
    }

    [Fact]
    public async Task ParallelExerciseLogs_KeepUserPointsConsistentWithLedger()
    {
        using var database = new SqliteFileDatabase();
        database.EnsureCreated();
        int exerciseId;
        int sessionId;
        await using (var seed = database.NewContext())
        {
            await TestData.AddUserAsync(seed, "player");
            exerciseId = (await TestData.AddExerciseAsync(seed, points: 40)).Id;
            var progression = new ProgressionService(seed);
            var session = await new DailySessionService(seed, progression, Achievements(seed, progression), Rules)
                .StartSessionAsync(new CreateDailySessionDto { SessionDate = DateTime.UtcNow }, "player");
            sessionId = session.Id;
        }

        await Task.WhenAll(Enumerable.Range(0, Parallelism).Select(_ => Task.Run(async () =>
        {
            await using var context = database.NewContext();
            var progression = new ProgressionService(context);
            var service = new DailySessionService(context, progression, Achievements(context, progression), Rules);
            await service.AddExerciseLogAsync(sessionId, new CreateUserExerciseLogDto
            {
                ExerciseId = exerciseId,
                Score = 100,
                DurationSeconds = 600,
                AccuracyPercent = 100
            }, "player");
        })));

        await using var check = database.NewContext();
        var user = await check.Users.SingleAsync(u => u.Id == "player");
        var ledger = await check.PointTransactions.Where(t => t.UserId == "player").SumAsync(t => t.Amount);
        Assert.Equal(Parallelism, await check.UserExerciseLogs.CountAsync());
        Assert.Equal(ledger, user.TotalPoints);
        Assert.Equal(ledger, user.CurrentBalance);
        Assert.Equal(Parallelism * 40, ledger);
    }
}
