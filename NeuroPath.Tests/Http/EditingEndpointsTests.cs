using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NeuroPath.Core.Common;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.Course;
using NeuroPath.Core.Models.DTOs.Quiz;
using NeuroPath.Core.Models.DTOs.Social;
using NeuroPath.Data.Context;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Http;

public class EditingEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public EditingEndpointsTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task DraftQuiz_CanBeEditedAndTotalPointsFollowTheQuestions()
    {
        var trainer = await _factory.CreateAccountAsync(AppRoles.Trainer);
        using var client = _factory.ClientFor(trainer.AccessToken);
        var quiz = await CreateQuizAsync(client);
        var first = await AddQuestionAsync(client, quiz.Id, 10);
        var second = await AddQuestionAsync(client, quiz.Id, 5);

        var update = await client.PutAsJsonAsync($"/api/quizzes/{quiz.Id}", QuizBody("Renamed quiz", await DifficultyIdAsync()));
        Assert.Equal(HttpStatusCode.OK, update.StatusCode);
        Assert.Equal("Renamed quiz", (await update.Content.ReadFromJsonAsync<QuizDto>())!.Title);

        var edit = await client.PutAsJsonAsync($"/api/quizzes/{quiz.Id}/questions/{first.Id}", QuestionBody(20));
        Assert.Equal(HttpStatusCode.OK, edit.StatusCode);
        Assert.Equal(25, (await GetQuizAsync(client, quiz.Id)).TotalPoints);

        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync($"/api/quizzes/{quiz.Id}/questions/{second.Id}")).StatusCode);
        var detail = await GetQuizAsync(client, quiz.Id);
        Assert.Equal(20, detail.TotalPoints);
        Assert.Single(detail.Questions);
        Assert.Equal(2, detail.Questions[0].Answers.Count);
    }

    [Fact]
    public async Task PublishedQuiz_CannotBeEditedUntilUnpublished()
    {
        var trainer = await _factory.CreateAccountAsync(AppRoles.Trainer);
        using var client = _factory.ClientFor(trainer.AccessToken);
        var quiz = await CreateQuizAsync(client);
        await AddQuestionAsync(client, quiz.Id, 10);
        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsync($"/api/quizzes/{quiz.Id}/publish", null)).StatusCode);

        var blocked = await client.PutAsJsonAsync($"/api/quizzes/{quiz.Id}", QuizBody("Changed", await DifficultyIdAsync()));
        Assert.Equal(HttpStatusCode.BadRequest, blocked.StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsync($"/api/quizzes/{quiz.Id}/unpublish", null)).StatusCode);
        var allowed = await client.PutAsJsonAsync($"/api/quizzes/{quiz.Id}", QuizBody("Changed", await DifficultyIdAsync()));
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
    }

    [Fact]
    public async Task QuizWithAttempts_CannotBeUnpublished()
    {
        var trainer = await _factory.CreateAccountAsync(AppRoles.Trainer);
        var player = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(trainer.AccessToken);
        using var playerClient = _factory.ClientFor(player.AccessToken);
        var quiz = await CreateQuizAsync(client);
        await AddQuestionAsync(client, quiz.Id, 10);
        await client.PostAsync($"/api/quizzes/{quiz.Id}/publish", null);

        Assert.Equal(HttpStatusCode.OK, (await playerClient.PostAsync($"/api/quizzes/{quiz.Id}/start", null)).StatusCode);

        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsync($"/api/quizzes/{quiz.Id}/unpublish", null)).StatusCode);
    }

    [Fact]
    public async Task AnotherTrainer_CannotEditSomeoneElsesQuiz()
    {
        var owner = await _factory.CreateAccountAsync(AppRoles.Trainer);
        var other = await _factory.CreateAccountAsync(AppRoles.Trainer);
        using var ownerClient = _factory.ClientFor(owner.AccessToken);
        using var otherClient = _factory.ClientFor(other.AccessToken);
        var quiz = await CreateQuizAsync(ownerClient);

        var response = await otherClient.PutAsJsonAsync($"/api/quizzes/{quiz.Id}", QuizBody("Hijacked", await DifficultyIdAsync()));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CompletedLesson_CannotBeDeleted_ButAnUntouchedOneCan()
    {
        var trainer = await _factory.CreateAccountAsync(AppRoles.Trainer);
        var player = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(trainer.AccessToken);
        using var playerClient = _factory.ClientFor(player.AccessToken);

        var created = await client.PostAsJsonAsync("/api/courses", new CreateCourseModuleDto
        {
            Title = $"Course {Guid.NewGuid():N}",
            ShortDescription = "Short",
            Description = "Long description",
            DifficultyLevelId = await DifficultyIdAsync(),
            DurationWeeks = 2,
            TotalPoints = 100
        });
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var course = (await created.Content.ReadFromJsonAsync<CourseModuleDto>())!;
        var first = await AddLessonAsync(client, course.Id, 0);
        var second = await AddLessonAsync(client, course.Id, 1);
        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsync($"/api/courses/{course.Id}/publish", null)).StatusCode);

        Assert.Equal(HttpStatusCode.OK, (await playerClient.PostAsync($"/api/courses/{course.Id}/enroll", null)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await playerClient.PostAsync($"/api/courses/{course.Id}/lessons/{first.Id}/complete", null)).StatusCode);

        var rename = await client.PutAsJsonAsync($"/api/courses/{course.Id}/lessons/{first.Id}", LessonBody(0, "Renamed lesson"));
        Assert.Equal(HttpStatusCode.OK, rename.StatusCode);

        Assert.Equal(HttpStatusCode.Conflict, (await client.DeleteAsync($"/api/courses/{course.Id}/lessons/{first.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync($"/api/courses/{course.Id}/lessons/{second.Id}")).StatusCode);
    }

    [Fact]
    public async Task TeamLeader_CanEditAndRemoveMembers_ButMembersCannot()
    {
        var leader = await _factory.CreateAccountAsync();
        var coLeader = await _factory.CreateAccountAsync();
        var member = await _factory.CreateAccountAsync();
        using var leaderClient = _factory.ClientFor(leader.AccessToken);
        using var coLeaderClient = _factory.ClientFor(coLeader.AccessToken);
        using var memberClient = _factory.ClientFor(member.AccessToken);

        var created = await leaderClient.PostAsJsonAsync("/api/teams", TeamBody($"Team {Guid.NewGuid():N}"[..20], 10));
        var team = (await created.Content.ReadFromJsonAsync<TeamDto>())!;
        await coLeaderClient.PostAsync($"/api/teams/{team.Id}/join", null);
        await memberClient.PostAsync($"/api/teams/{team.Id}/join", null);
        var promote = await leaderClient.PutAsJsonAsync($"/api/teams/{team.Id}/members/{coLeader.Id}/role", new { role = "CoLeader" });
        Assert.Equal(HttpStatusCode.NoContent, promote.StatusCode);

        Assert.Equal(HttpStatusCode.BadRequest, (await leaderClient.PutAsJsonAsync($"/api/teams/{team.Id}", TeamBody(team.Name, 2))).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await coLeaderClient.PutAsJsonAsync($"/api/teams/{team.Id}", TeamBody(team.Name, 20))).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await leaderClient.PutAsJsonAsync($"/api/teams/{team.Id}", TeamBody(team.Name, 20))).StatusCode);

        Assert.Equal(HttpStatusCode.Forbidden, (await memberClient.DeleteAsync($"/api/teams/{team.Id}/members/{coLeader.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await coLeaderClient.DeleteAsync($"/api/teams/{team.Id}/members/{leader.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await coLeaderClient.DeleteAsync($"/api/teams/{team.Id}/members/{member.Id}")).StatusCode);

        Assert.Equal(HttpStatusCode.Forbidden, (await coLeaderClient.DeleteAsync($"/api/teams/{team.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await leaderClient.DeleteAsync($"/api/teams/{team.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await memberClient.PostAsync($"/api/teams/{team.Id}/join", null)).StatusCode);
    }

    [Fact]
    public async Task DeletingAnAccount_AnonymizesItAndBlocksLogin()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);

        var wrong = await SendDeleteAsync(client, "Wrong#Pass2026");
        Assert.Equal(HttpStatusCode.BadRequest, wrong.StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/users/me")).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await SendDeleteAsync(client, ApiFactory.UserPassword)).StatusCode);

        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/users/me")).StatusCode);
        var login = await _factory.CreateClient().PostAsJsonAsync("/api/auth/login", new { email = account.Email, password = ApiFactory.UserPassword });
        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);
        var refresh = await _factory.CreateClient().PostAsJsonAsync("/api/auth/refresh", new { refreshToken = account.RefreshToken });
        Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);

        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<NeuroPathDbContext>();
        var user = await db.Users.AsNoTracking().SingleAsync(u => u.Id == account.Id);
        Assert.False(user.IsActive);
        Assert.EndsWith("@deleted.invalid", user.Email);
        Assert.StartsWith("deleted-", user.UserName);
        Assert.Null(user.PasswordHash);
        Assert.False(await db.RefreshTokens.AnyAsync(t => t.UserId == account.Id));
        var profile = await db.UserProfiles.AsNoTracking().SingleOrDefaultAsync(p => p.UserId == account.Id);
        if (profile != null) Assert.Equal("Deleted", profile.FirstName);
    }

    [Fact]
    public async Task LastAdministrator_CannotDeleteTheirAccount()
    {
        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var admins = await users.GetUsersInRoleAsync(AppRoles.Admin);
            if (admins.Count(a => a.IsActive) > 1) return;
        }

        var admin = await _factory.LoginAdminAsync();
        using var client = _factory.ClientFor(admin.AccessToken);

        var response = await SendDeleteAsync(client, ApiFactory.AdminPassword);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static Task<HttpResponseMessage> SendDeleteAsync(HttpClient client, string password)
        => client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/api/users/me") { Content = JsonContent.Create(new { password }) });

    private async Task<int> DifficultyIdAsync()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<NeuroPathDbContext>();
        return await db.DifficultyLevels.OrderBy(d => d.Level).Select(d => d.Id).FirstAsync();
    }

    private async Task<QuizDto> CreateQuizAsync(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/quizzes", QuizBody("Draft quiz", await DifficultyIdAsync()));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<QuizDto>())!;
    }

    private static async Task<QuizQuestionDto> AddQuestionAsync(HttpClient client, int quizId, int points)
    {
        var response = await client.PostAsJsonAsync($"/api/quizzes/{quizId}/questions", QuestionBody(points));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<QuizQuestionDto>())!;
    }

    private static async Task<QuizDetailDto> GetQuizAsync(HttpClient client, int quizId)
        => (await client.GetFromJsonAsync<QuizDetailDto>($"/api/quizzes/{quizId}"))!;

    private static async Task<ModuleLessonDto> AddLessonAsync(HttpClient client, int courseId, int order)
    {
        var response = await client.PostAsJsonAsync($"/api/courses/{courseId}/lessons", LessonBody(order, $"Lesson {order}"));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<ModuleLessonDto>())!;
    }

    private static CreateQuizDto QuizBody(string title, int difficultyId) => new()
    {
        Title = title,
        Description = "Quiz description",
        DifficultyLevelId = difficultyId,
        PassingScorePercent = 60
    };

    private static CreateQuizQuestionDto QuestionBody(int points) => new()
    {
        QuestionText = "Which one is right?",
        Points = points,
        OrderIndex = 0,
        Answers =
        [
            new CreateQuizAnswerDto { AnswerText = "Right", IsCorrect = true, OrderIndex = 0 },
            new CreateQuizAnswerDto { AnswerText = "Wrong", IsCorrect = false, OrderIndex = 1 }
        ]
    };

    private static CreateModuleLessonDto LessonBody(int order, string title) => new()
    {
        Title = title,
        Content = "Lesson content",
        DurationMinutes = 5,
        OrderIndex = order,
        Points = 10
    };

    private static CreateTeamDto TeamBody(string name, int maxMembers) => new()
    {
        Name = name,
        Description = "Team description",
        MaxMembers = maxMembers
    };
}
