using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.DTOs.Quiz;
using NeuroPath.Core.Services;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class QuizServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    private QuizService CreateService()
    {
        var progression = new ProgressionService(_database.Context);
        var achievements = new AchievementService(_database.Context, _database.Cache, progression);
        return new QuizService(_database.Context, progression, achievements);
    }

    [Fact]
    public async Task Submit_DuplicateAnswersForSameQuestion_AreCountedOnce()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author");
        var first = quiz.Questions.First(q => q.OrderIndex == 1);
        var correct = TestData.CorrectAnswerId(first);

        var dto = new SubmitQuizDto
        {
            QuizId = quiz.Id,
            Answers = Enumerable.Range(0, 5).Select(_ => new UserAnswerSubmission { QuestionId = first.Id, SelectedAnswerId = correct }).ToList()
        };

        var result = await CreateService().SubmitAsync(dto, "player");

        Assert.Equal(10, result.Score);
        Assert.Equal(15, result.MaxScore);
        Assert.True(result.ScorePercent <= 100);
    }

    [Fact]
    public async Task Submit_AwardsPointsOnlyOnFirstPassedAttempt()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author", multiplier: 1.5m);
        var answers = quiz.Questions
            .Select(q => new UserAnswerSubmission { QuestionId = q.Id, SelectedAnswerId = TestData.CorrectAnswerId(q) })
            .ToList();
        var service = CreateService();

        var first = await service.SubmitAsync(new SubmitQuizDto { QuizId = quiz.Id, Answers = answers }, "player");
        var second = await service.SubmitAsync(new SubmitQuizDto { QuizId = quiz.Id, Answers = answers }, "player");

        Assert.True(first.IsPassed);
        Assert.Equal(23, first.PointsAwarded);
        Assert.Equal(0, second.PointsAwarded);
        Assert.Equal(2, second.AttemptNumber);

        await using var check = _database.NewContext();
        var user = await check.Users.SingleAsync(u => u.Id == "player");
        Assert.Equal(23, user.TotalPoints);
        Assert.Equal(23, user.CurrentBalance);
        Assert.Equal(23, await check.PointTransactions.Where(t => t.UserId == "player").SumAsync(t => t.Amount));
    }

    [Fact]
    public async Task Submit_FailedAttempt_AwardsNoPoints()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author");
        var answers = quiz.Questions
            .Select(q => new UserAnswerSubmission { QuestionId = q.Id, SelectedAnswerId = TestData.WrongAnswerId(q) })
            .ToList();

        var result = await CreateService().SubmitAsync(new SubmitQuizDto { QuizId = quiz.Id, Answers = answers }, "player");

        Assert.False(result.IsPassed);
        Assert.Equal(0, result.PointsAwarded);
    }

    [Fact]
    public async Task Submit_AnswerFromAnotherQuestion_IsNotAccepted()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author");
        var first = quiz.Questions.First(q => q.OrderIndex == 1);
        var second = quiz.Questions.First(q => q.OrderIndex == 2);

        var dto = new SubmitQuizDto
        {
            QuizId = quiz.Id,
            Answers = new List<UserAnswerSubmission> { new() { QuestionId = first.Id, SelectedAnswerId = TestData.CorrectAnswerId(second) } }
        };

        var result = await CreateService().SubmitAsync(dto, "player");

        Assert.Equal(0, result.Score);
    }

    [Fact]
    public async Task Submit_UnpublishedQuiz_ThrowsNotFound()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author");
        quiz.IsPublished = false;
        await _database.Context.SaveChangesAsync();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CreateService().SubmitAsync(new SubmitQuizDto { QuizId = quiz.Id, Answers = new() }, "player"));
    }

    [Fact]
    public async Task Submit_TimedQuizWithoutStart_IsRejected()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author", timeLimitMinutes: 5);
        var answers = quiz.Questions
            .Select(q => new UserAnswerSubmission { QuestionId = q.Id, SelectedAnswerId = TestData.CorrectAnswerId(q) })
            .ToList();
        var service = CreateService();

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.SubmitAsync(new SubmitQuizDto { QuizId = quiz.Id, Answers = answers }, "player"));

        var start = await service.StartAsync(quiz.Id, "player");
        var result = await service.SubmitAsync(new SubmitQuizDto { QuizId = quiz.Id, AttemptId = start.AttemptId, Answers = answers }, "player");

        Assert.NotNull(start.ExpiresAt);
        Assert.True(result.IsPassed);
        Assert.Equal(start.AttemptId, result.Id);
    }

    [Fact]
    public async Task Start_CalledTwice_ReturnsSameOpenAttempt()
    {
        await TestData.AddUserAsync(_database.Context, "author");
        await TestData.AddUserAsync(_database.Context, "player");
        var quiz = await TestData.AddPublishedQuizAsync(_database.Context, "author", timeLimitMinutes: 5);
        var service = CreateService();

        var first = await service.StartAsync(quiz.Id, "player");
        var second = await service.StartAsync(quiz.Id, "player");

        Assert.Equal(first.AttemptId, second.AttemptId);
    }

    [Fact]
    public async Task GetAll_ShowsDraftsOnlyToAdminsAndTheirAuthors()
    {
        await TestData.AddUserAsync(_database.Context, "trainer-a");
        await TestData.AddUserAsync(_database.Context, "trainer-b");
        var published = await TestData.AddPublishedQuizAsync(_database.Context, "trainer-a");
        var draftA = await TestData.AddPublishedQuizAsync(_database.Context, "trainer-a");
        var draftB = await TestData.AddPublishedQuizAsync(_database.Context, "trainer-b");
        var archived = await TestData.AddPublishedQuizAsync(_database.Context, "trainer-a");
        draftA.IsPublished = false;
        draftB.IsPublished = false;
        archived.IsPublished = false;
        archived.IsActive = false;
        await _database.Context.SaveChangesAsync();
        var service = CreateService();

        var anonymous = (await service.GetAllAsync()).Select(q => q.Id).ToList();
        var ignoredFlag = (await service.GetAllAsync(includeDrafts: false, userId: "trainer-a")).Select(q => q.Id).ToList();
        var trainer = (await service.GetAllAsync(includeDrafts: true, userId: "trainer-a")).Select(q => q.Id).ToList();
        var admin = (await service.GetAllAsync(includeDrafts: true, userId: "admin", isAdmin: true)).Select(q => q.Id).ToList();

        Assert.Equal(new[] { published.Id }, anonymous);
        Assert.Equal(new[] { published.Id }, ignoredFlag);
        Assert.Equal(new[] { draftA.Id, published.Id }.OrderBy(x => x), trainer.OrderBy(x => x));
        Assert.Equal(new[] { draftA.Id, draftB.Id, published.Id }.OrderBy(x => x), admin.OrderBy(x => x));
        Assert.DoesNotContain(archived.Id, admin);
        Assert.False((await service.GetAllAsync(includeDrafts: true, userId: "trainer-a")).First().IsPublished);
    }
}
