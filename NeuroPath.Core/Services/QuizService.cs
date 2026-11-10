using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Core.Models.DTOs.Quiz;

namespace NeuroPath.Core.Services;

public class QuizService : IQuizService
{
    private const int TimeLimitGraceSeconds = 30;

    private readonly INeuroPathDbContext _db;
    private readonly IProgressionService _progression;
    private readonly IAchievementService _achievements;

    public QuizService(INeuroPathDbContext db, IProgressionService progression, IAchievementService achievements)
    {
        _db = db;
        _progression = progression;
        _achievements = achievements;
    }

    public async Task<List<QuizDto>> GetAllAsync(bool includeDrafts = false, string? userId = null, bool isAdmin = false)
        => await _db.Quizzes.AsNoTracking()
            .Where(q => q.IsActive && (q.IsPublished || (includeDrafts && (isAdmin || q.CreatedByUserId == userId))))
            .OrderBy(q => q.IsPublished)
            .ThenByDescending(q => q.CreatedAt)
            .Select(ToQuizDto)
            .ToListAsync();

    public async Task<QuizDetailDto?> GetByIdAsync(int id, bool includeUnpublished = false)
        => await _db.Quizzes.AsNoTracking()
            .Where(q => q.Id == id && (includeUnpublished || (q.IsPublished && q.IsActive)))
            .Select(q => new QuizDetailDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                DifficultyLevelId = q.DifficultyLevelId,
                TimeLimitMinutes = q.TimeLimitMinutes,
                PassingScorePercent = q.PassingScorePercent,
                TotalPoints = q.TotalPoints,
                IsActive = q.IsActive,
                IsPublished = q.IsPublished,
                Questions = q.Questions.OrderBy(x => x.OrderIndex).Select(x => new QuizQuestionDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    QuestionType = x.QuestionType,
                    Points = x.Points,
                    OrderIndex = x.OrderIndex,
                    ImageUrl = x.ImageUrl,
                    Answers = x.Answers.OrderBy(a => a.OrderIndex).Select(a => new QuizAnswerDto
                    {
                        Id = a.Id,
                        AnswerText = a.AnswerText,
                        OrderIndex = a.OrderIndex
                    }).ToList()
                }).ToList()
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync();

    public async Task<QuizDto> CreateAsync(CreateQuizDto dto, string userId)
    {
        if (!await _db.DifficultyLevels.AnyAsync(d => d.Id == dto.DifficultyLevelId))
            throw new BusinessRuleException($"Difficulty level {dto.DifficultyLevelId} does not exist.");

        var quiz = new Quiz
        {
            Title = dto.Title,
            Description = dto.Description,
            DifficultyLevelId = dto.DifficultyLevelId,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            PassingScorePercent = dto.PassingScorePercent,
            CreatedByUserId = userId
        };
        _db.Quizzes.Add(quiz);
        await _db.SaveChangesAsync();

        return await _db.Quizzes.AsNoTracking()
            .Where(q => q.Id == quiz.Id)
            .Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                DifficultyName = q.DifficultyLevel.Name,
                TimeLimitMinutes = q.TimeLimitMinutes,
                PassingScorePercent = q.PassingScorePercent,
                TotalPoints = q.TotalPoints,
                IsActive = q.IsActive,
                IsPublished = q.IsPublished,
                QuestionCount = 0,
                CreatedAt = q.CreatedAt
            })
            .FirstAsync();
    }

    public async Task<QuizQuestionDto> AddQuestionAsync(int quizId, CreateQuizQuestionDto dto, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId)
            ?? throw new NotFoundException("Quiz not found.");
        EnsureCanModify(quiz, userId, isAdmin);
        if (quiz.IsPublished)
            throw new BusinessRuleException("Questions cannot be added to a published quiz.");

        var question = new QuizQuestion
        {
            QuizId = quizId,
            QuestionText = dto.QuestionText,
            QuestionType = dto.QuestionType,
            Points = dto.Points,
            OrderIndex = dto.OrderIndex,
            Explanation = dto.Explanation,
            ImageUrl = dto.ImageUrl
        };
        foreach (var answer in dto.Answers)
            question.Answers.Add(new QuizAnswer { AnswerText = answer.AnswerText, IsCorrect = answer.IsCorrect, OrderIndex = answer.OrderIndex });

        _db.QuizQuestions.Add(question);
        quiz.TotalPoints += question.Points;
        await _db.SaveChangesAsync();

        return ToQuestionDto(question);
    }

    public async Task<QuizDto?> UpdateAsync(int id, CreateQuizDto dto, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == id && q.IsActive);
        if (quiz == null) return null;
        EnsureCanModify(quiz, userId, isAdmin);
        EnsureDraft(quiz);
        if (!await _db.DifficultyLevels.AnyAsync(d => d.Id == dto.DifficultyLevelId))
            throw new BusinessRuleException($"Difficulty level {dto.DifficultyLevelId} does not exist.");

        quiz.Title = dto.Title;
        quiz.Description = dto.Description;
        quiz.DifficultyLevelId = dto.DifficultyLevelId;
        quiz.TimeLimitMinutes = dto.TimeLimitMinutes;
        quiz.PassingScorePercent = dto.PassingScorePercent;
        await _db.SaveChangesAsync();

        return await _db.Quizzes.AsNoTracking().Where(q => q.Id == id).Select(ToQuizDto).FirstAsync();
    }

    public async Task<QuizQuestionDto?> UpdateQuestionAsync(int quizId, int questionId, CreateQuizQuestionDto dto, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId && q.IsActive)
            ?? throw new NotFoundException("Quiz not found.");
        EnsureCanModify(quiz, userId, isAdmin);
        EnsureDraft(quiz);

        var question = await _db.QuizQuestions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == questionId && q.QuizId == quizId);
        if (question == null) return null;

        question.QuestionText = dto.QuestionText;
        question.QuestionType = dto.QuestionType;
        question.Points = dto.Points;
        question.OrderIndex = dto.OrderIndex;
        question.Explanation = dto.Explanation;
        question.ImageUrl = dto.ImageUrl;
        _db.QuizAnswers.RemoveRange(question.Answers);
        question.Answers = dto.Answers
            .Select(a => new QuizAnswer { AnswerText = a.AnswerText, IsCorrect = a.IsCorrect, OrderIndex = a.OrderIndex })
            .ToList();

        await using var transaction = await _db.Database.BeginTransactionAsync();
        await _db.SaveChangesAsync();
        await RecalculateTotalPointsAsync(quizId);
        await transaction.CommitAsync();

        return ToQuestionDto(question);
    }

    public async Task<bool> DeleteQuestionAsync(int quizId, int questionId, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId && q.IsActive);
        if (quiz == null) return false;
        EnsureCanModify(quiz, userId, isAdmin);
        EnsureDraft(quiz);

        var question = await _db.QuizQuestions.FirstOrDefaultAsync(q => q.Id == questionId && q.QuizId == quizId);
        if (question == null) return false;

        await using var transaction = await _db.Database.BeginTransactionAsync();
        _db.QuizQuestions.Remove(question);
        await _db.SaveChangesAsync();
        await RecalculateTotalPointsAsync(quizId);
        await transaction.CommitAsync();
        return true;
    }

    public async Task<bool> UnpublishAsync(int id, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == id && q.IsActive);
        if (quiz == null) return false;
        EnsureCanModify(quiz, userId, isAdmin);
        if (!quiz.IsPublished) return true;
        if (await _db.UserQuizAttempts.AnyAsync(a => a.QuizId == id))
            throw new BusinessRuleException("This quiz already has attempts. Delete it to archive it and create a new version instead.");

        quiz.IsPublished = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PublishAsync(int id, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
        if (quiz == null) return false;
        EnsureCanModify(quiz, userId, isAdmin);

        var questions = await _db.QuizQuestions
            .Where(q => q.QuizId == id)
            .Select(q => new { HasCorrect = q.Answers.Any(a => a.IsCorrect) })
            .ToListAsync();
        if (questions.Count == 0)
            throw new BusinessRuleException("A quiz needs at least one question before it can be published.");
        if (questions.Any(q => !q.HasCorrect))
            throw new BusinessRuleException("Every question needs at least one correct answer.");

        quiz.IsPublished = true;
        quiz.IsActive = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
        if (quiz == null) return false;
        EnsureCanModify(quiz, userId, isAdmin);

        if (await _db.UserQuizAttempts.AnyAsync(a => a.QuizId == id))
        {
            quiz.IsActive = false;
            quiz.IsPublished = false;
        }
        else
        {
            _db.Quizzes.Remove(quiz);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<QuizStartDto> StartAsync(int quizId, string userId)
    {
        var quiz = await _db.Quizzes.AsNoTracking()
            .Where(q => q.Id == quizId && q.IsPublished && q.IsActive)
            .Select(q => new { q.Id, q.TimeLimitMinutes, MaxScore = q.Questions.Sum(x => x.Points) })
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Quiz not found.");

        var now = DateTime.UtcNow;
        var open = await _db.UserQuizAttempts
            .Where(a => a.UserId == userId && a.QuizId == quizId && a.CompletedAt == null)
            .OrderByDescending(a => a.StartedAt)
            .FirstOrDefaultAsync();

        if (open != null)
        {
            var stillRunning = quiz.TimeLimitMinutes is not > 0 || open.StartedAt.AddMinutes(quiz.TimeLimitMinutes.Value) > now;
            if (stillRunning) return ToStartDto(open, quiz.TimeLimitMinutes);
            open.CompletedAt = now;
        }

        var attempt = new UserQuizAttempt
        {
            UserId = userId,
            QuizId = quizId,
            StartedAt = now,
            MaxScore = quiz.MaxScore,
            AttemptNumber = await _db.UserQuizAttempts.CountAsync(a => a.UserId == userId && a.QuizId == quizId) + 1
        };
        _db.UserQuizAttempts.Add(attempt);
        await _db.SaveChangesAsync();
        return ToStartDto(attempt, quiz.TimeLimitMinutes);
    }

    public async Task<QuizAttemptResultDto> SubmitAsync(SubmitQuizDto dto, string userId)
    {
        var quiz = await _db.Quizzes.AsNoTracking()
            .Include(q => q.DifficultyLevel)
            .Include(q => q.Questions).ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .FirstOrDefaultAsync(q => q.Id == dto.QuizId && q.IsPublished && q.IsActive)
            ?? throw new NotFoundException("Quiz not found.");

        var submitted = dto.Answers
            .GroupBy(a => a.QuestionId)
            .ToDictionary(g => g.Key, g => g.First().SelectedAnswerId);

        var maxScore = quiz.Questions.Sum(q => q.Points);
        var passedBefore = await _db.UserQuizAttempts.AnyAsync(a => a.UserId == userId && a.QuizId == quiz.Id && a.IsPassed);
        var isTimed = quiz.TimeLimitMinutes is > 0;

        UserQuizAttempt attempt;
        var isNewAttempt = false;
        if (dto.AttemptId.HasValue)
        {
            attempt = await _db.UserQuizAttempts.FirstOrDefaultAsync(a => a.Id == dto.AttemptId && a.UserId == userId && a.QuizId == quiz.Id && a.CompletedAt == null)
                ?? throw new NotFoundException("Open quiz attempt not found.");

            if (isTimed && DateTime.UtcNow > attempt.StartedAt.AddMinutes(quiz.TimeLimitMinutes!.Value).AddSeconds(TimeLimitGraceSeconds))
            {
                attempt.CompletedAt = DateTime.UtcNow;
                attempt.MaxScore = maxScore;
                attempt.Score = 0;
                attempt.ScorePercent = 0;
                attempt.IsPassed = false;
                await _db.SaveChangesAsync();
                throw new BusinessRuleException("The time limit for this quiz was exceeded.");
            }
        }
        else
        {
            if (isTimed)
                throw new BusinessRuleException("This quiz is timed. Start it before submitting.");

            var attemptNumber = await _db.UserQuizAttempts.CountAsync(a => a.UserId == userId && a.QuizId == quiz.Id) + 1;
            attempt = new UserQuizAttempt { UserId = userId, QuizId = quiz.Id, AttemptNumber = attemptNumber };
            isNewAttempt = true;
        }

        attempt.MaxScore = maxScore;
        var results = new List<QuizAnswerResultDto>();
        var score = 0;

        foreach (var question in quiz.Questions.OrderBy(q => q.OrderIndex))
        {
            submitted.TryGetValue(question.Id, out var selectedId);
            var selectedIsValid = selectedId.HasValue && question.Answers.Any(a => a.Id == selectedId);
            var correct = selectedIsValid && question.Answers.Any(a => a.Id == selectedId && a.IsCorrect);
            if (correct) score += question.Points;

            attempt.UserAnswers.Add(new UserQuizAnswer
            {
                QuizQuestionId = question.Id,
                SelectedAnswerId = selectedIsValid ? selectedId : null,
                IsCorrect = correct
            });
            results.Add(new QuizAnswerResultDto
            {
                QuestionId = question.Id,
                QuestionText = question.QuestionText,
                SelectedAnswerId = selectedIsValid ? selectedId : null,
                IsCorrect = correct,
                Explanation = question.Explanation
            });
        }

        attempt.Score = score;
        attempt.ScorePercent = maxScore > 0 ? Math.Round((decimal)score / maxScore * 100, 2) : 0;
        attempt.IsPassed = attempt.ScorePercent >= quiz.PassingScorePercent;
        attempt.CompletedAt = DateTime.UtcNow;
        attempt.PointsAwarded = attempt.IsPassed && !passedBefore
            ? (int)Math.Round(score * quiz.DifficultyLevel.PointMultiplier, MidpointRounding.AwayFromZero)
            : 0;

        try
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            if (isNewAttempt) _db.UserQuizAttempts.Add(attempt);
            await _progression.AwardPointsAsync(userId, attempt.PointsAwarded, PointSource.Quiz, quiz.Id);
            await _progression.RegisterActivityAsync(userId);
            if (attempt.PointsAwarded > 0)
                await _progression.ApplyChallengeProgressAsync(userId, attempt.PointsAwarded, countsAsExercise: false, accuracy: null);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException) when (attempt.PointsAwarded > 0)
        {
            throw new ConflictException("This quiz was already completed by another request. Refresh your history.");
        }

        if (attempt.IsPassed)
            await _achievements.CheckAndAwardAsync(userId);

        return new QuizAttemptResultDto
        {
            Id = attempt.Id,
            QuizId = quiz.Id,
            QuizTitle = quiz.Title,
            Score = score,
            MaxScore = maxScore,
            ScorePercent = attempt.ScorePercent,
            IsPassed = attempt.IsPassed,
            AttemptNumber = attempt.AttemptNumber,
            PointsAwarded = attempt.PointsAwarded,
            StartedAt = attempt.StartedAt,
            CompletedAt = attempt.CompletedAt,
            AnswerResults = results
        };
    }

    public async Task<List<QuizAttemptResultDto>> GetUserHistoryAsync(string userId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.UserQuizAttempts.AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.StartedAt)
            .Skip(skip).Take(take)
            .Select(a => new QuizAttemptResultDto
            {
                Id = a.Id,
                QuizId = a.QuizId,
                QuizTitle = a.Quiz.Title,
                Score = a.Score,
                MaxScore = a.MaxScore,
                ScorePercent = a.ScorePercent,
                IsPassed = a.IsPassed,
                AttemptNumber = a.AttemptNumber,
                PointsAwarded = a.PointsAwarded,
                StartedAt = a.StartedAt,
                CompletedAt = a.CompletedAt
            })
            .ToListAsync();
    }

    private static QuizStartDto ToStartDto(UserQuizAttempt attempt, int? timeLimitMinutes) => new()
    {
        AttemptId = attempt.Id,
        QuizId = attempt.QuizId,
        AttemptNumber = attempt.AttemptNumber,
        StartedAt = attempt.StartedAt,
        ExpiresAt = timeLimitMinutes is > 0 ? attempt.StartedAt.AddMinutes(timeLimitMinutes.Value) : null
    };

    private static QuizQuestionDto ToQuestionDto(QuizQuestion question) => new()
    {
        Id = question.Id,
        QuestionText = question.QuestionText,
        QuestionType = question.QuestionType,
        Points = question.Points,
        OrderIndex = question.OrderIndex,
        ImageUrl = question.ImageUrl,
        Answers = question.Answers.OrderBy(a => a.OrderIndex)
            .Select(a => new QuizAnswerDto { Id = a.Id, AnswerText = a.AnswerText, OrderIndex = a.OrderIndex })
            .ToList()
    };

    private async Task RecalculateTotalPointsAsync(int quizId)
    {
        var total = await _db.QuizQuestions.Where(q => q.QuizId == quizId).SumAsync(q => (int?)q.Points) ?? 0;
        await _db.Quizzes.Where(q => q.Id == quizId).ExecuteUpdateAsync(s => s.SetProperty(q => q.TotalPoints, total));
    }

    private static readonly Expression<Func<Quiz, QuizDto>> ToQuizDto = q => new QuizDto
    {
        Id = q.Id,
        Title = q.Title,
        Description = q.Description,
        DifficultyName = q.DifficultyLevel.Name,
        TimeLimitMinutes = q.TimeLimitMinutes,
        PassingScorePercent = q.PassingScorePercent,
        TotalPoints = q.TotalPoints,
        IsActive = q.IsActive,
        IsPublished = q.IsPublished,
        QuestionCount = q.Questions.Count,
        CreatedAt = q.CreatedAt
    };

    private static void EnsureDraft(Quiz quiz)
    {
        if (quiz.IsPublished)
            throw new BusinessRuleException("A published quiz cannot be edited. Unpublish it first.");
    }

    private static void EnsureCanModify(Quiz quiz, string userId, bool isAdmin)
    {
        if (!isAdmin && quiz.CreatedByUserId != userId)
            throw new ForbiddenException("Only the creator or an administrator can modify this quiz.");
    }
}
