using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Core.Models.DTOs.Quiz;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class QuizService : IQuizService
{
    private readonly NeuroPathDbContext _db;

    public QuizService(NeuroPathDbContext db) => _db = db;

    public async Task<List<QuizDto>> GetAllAsync()
        => await _db.Quizzes.Where(q => q.IsPublished && q.IsActive)
            .Include(q => q.DifficultyLevel)
            .Select(q => new QuizDto
            {
                Id = q.Id, Title = q.Title, Description = q.Description,
                DifficultyName = q.DifficultyLevel.Name, TimeLimitMinutes = q.TimeLimitMinutes,
                PassingScorePercent = q.PassingScorePercent, TotalPoints = q.TotalPoints,
                IsActive = q.IsActive, IsPublished = q.IsPublished,
                QuestionCount = q.Questions.Count, CreatedAt = q.CreatedAt
            }).ToListAsync();

    public async Task<QuizDetailDto?> GetByIdAsync(int id)
    {
        var q = await _db.Quizzes.Include(x => x.DifficultyLevel)
            .Include(x => x.Questions).ThenInclude(x => x.Answers)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (q == null) return null;
        return new QuizDetailDto
        {
            Id = q.Id, Title = q.Title, Description = q.Description,
            DifficultyLevelId = q.DifficultyLevelId, TimeLimitMinutes = q.TimeLimitMinutes,
            PassingScorePercent = q.PassingScorePercent, TotalPoints = q.TotalPoints,
            IsActive = q.IsActive, IsPublished = q.IsPublished,
            Questions = q.Questions.OrderBy(x => x.OrderIndex).Select(ques => new QuizQuestionDto
            {
                Id = ques.Id, QuestionText = ques.QuestionText, QuestionType = ques.QuestionType,
                Points = ques.Points, OrderIndex = ques.OrderIndex, ImageUrl = ques.ImageUrl,
                Answers = ques.Answers.OrderBy(a => a.OrderIndex).Select(a => new QuizAnswerDto
                {
                    Id = a.Id, AnswerText = a.AnswerText, OrderIndex = a.OrderIndex
                }).ToList()
            }).ToList()
        };
    }

    public async Task<QuizDto> CreateAsync(CreateQuizDto dto, string userId)
    {
        var q = new Quiz
        {
            Title = dto.Title, Description = dto.Description, DifficultyLevelId = dto.DifficultyLevelId,
            TimeLimitMinutes = dto.TimeLimitMinutes, PassingScorePercent = dto.PassingScorePercent,
            TotalPoints = dto.TotalPoints, CreatedByUserId = userId
        };
        _db.Quizzes.Add(q);
        await _db.SaveChangesAsync();
        await _db.Entry(q).Reference(x => x.DifficultyLevel).LoadAsync();
        return new QuizDto
        {
            Id = q.Id, Title = q.Title, Description = q.Description,
            DifficultyName = q.DifficultyLevel.Name, TimeLimitMinutes = q.TimeLimitMinutes,
            PassingScorePercent = q.PassingScorePercent, TotalPoints = q.TotalPoints,
            IsActive = q.IsActive, IsPublished = q.IsPublished, CreatedAt = q.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var q = await _db.Quizzes.FindAsync(id);
        if (q == null) return false;
        _db.Quizzes.Remove(q);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<QuizAttemptResultDto> SubmitAsync(SubmitQuizDto dto, string userId)
    {
        var quiz = await _db.Quizzes.Include(q => q.Questions).ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == dto.QuizId)
            ?? throw new InvalidOperationException("Quiz not found.");

        var attemptCount = await _db.UserQuizAttempts.CountAsync(x => x.UserId == userId && x.QuizId == dto.QuizId);
        var attempt = new UserQuizAttempt
        {
            UserId = userId, QuizId = dto.QuizId,
            AttemptNumber = attemptCount + 1, MaxScore = quiz.TotalPoints
        };
        _db.UserQuizAttempts.Add(attempt);
        await _db.SaveChangesAsync();

        int score = 0;
        var results = new List<QuizAnswerResultDto>();
        foreach (var submission in dto.Answers)
        {
            var question = quiz.Questions.FirstOrDefault(q => q.Id == submission.QuestionId);
            if (question == null) continue;
            var correct = submission.SelectedAnswerId.HasValue
                && question.Answers.Any(a => a.Id == submission.SelectedAnswerId && a.IsCorrect);
            if (correct) score += question.Points;
            var userAnswer = new UserQuizAnswer
            {
                UserQuizAttemptId = attempt.Id, QuizQuestionId = submission.QuestionId,
                SelectedAnswerId = submission.SelectedAnswerId, IsCorrect = correct
            };
            _db.UserQuizAnswers.Add(userAnswer);
            results.Add(new QuizAnswerResultDto
            {
                QuestionId = question.Id, QuestionText = question.QuestionText,
                SelectedAnswerId = submission.SelectedAnswerId, IsCorrect = correct,
                Explanation = question.Explanation
            });
        }

        attempt.Score = score;
        attempt.ScorePercent = quiz.TotalPoints > 0 ? (decimal)score / quiz.TotalPoints * 100 : 0;
        attempt.IsPassed = attempt.ScorePercent >= quiz.PassingScorePercent;
        attempt.CompletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new QuizAttemptResultDto
        {
            Id = attempt.Id, QuizId = dto.QuizId, QuizTitle = quiz.Title,
            Score = score, MaxScore = quiz.TotalPoints, ScorePercent = attempt.ScorePercent,
            IsPassed = attempt.IsPassed, AttemptNumber = attempt.AttemptNumber,
            StartedAt = attempt.StartedAt, CompletedAt = attempt.CompletedAt,
            AnswerResults = results
        };
    }

    public async Task<List<QuizAttemptResultDto>> GetUserHistoryAsync(string userId)
        => await _db.UserQuizAttempts.Where(a => a.UserId == userId)
            .Include(a => a.Quiz)
            .OrderByDescending(a => a.StartedAt)
            .Select(a => new QuizAttemptResultDto
            {
                Id = a.Id, QuizId = a.QuizId, QuizTitle = a.Quiz.Title,
                Score = a.Score, MaxScore = a.MaxScore, ScorePercent = a.ScorePercent,
                IsPassed = a.IsPassed, AttemptNumber = a.AttemptNumber,
                StartedAt = a.StartedAt, CompletedAt = a.CompletedAt
            }).ToListAsync();
}
