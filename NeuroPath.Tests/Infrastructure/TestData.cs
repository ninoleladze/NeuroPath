using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Data.Context;

namespace NeuroPath.Tests.Infrastructure;

public static class TestData
{
    public static async Task<User> AddUserAsync(NeuroPathDbContext db, string id, int balance = 0, int totalPoints = 0)
    {
        var user = new User
        {
            Id = id,
            UserName = id,
            NormalizedUserName = id.ToUpperInvariant(),
            Email = $"{id}@test.local",
            NormalizedEmail = $"{id}@TEST.LOCAL",
            IsActive = true,
            IsEmailVerified = true,
            CurrentBalance = balance,
            TotalPoints = totalPoints
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public static async Task<Streak> AddStreakAsync(NeuroPathDbContext db, string userId, int current, DateTime lastActivity, int freezes = 0)
    {
        var streak = new Streak
        {
            UserId = userId,
            CurrentStreak = current,
            LongestStreak = current,
            TotalActiveDays = current,
            LastActivityDate = lastActivity,
            StreakFreezeCount = freezes
        };
        db.Streaks.Add(streak);
        await db.SaveChangesAsync();
        return streak;
    }

    public static async Task<Quiz> AddPublishedQuizAsync(NeuroPathDbContext db, string authorId, decimal multiplier = 1.5m, int? timeLimitMinutes = null)
    {
        var difficulty = new DifficultyLevel { Name = $"Level-{Guid.NewGuid():N}", Level = Random.Shared.Next(1000, 1_000_000), PointMultiplier = multiplier };
        var quiz = new Quiz
        {
            Title = "Memory basics",
            Description = "Test quiz",
            DifficultyLevel = difficulty,
            CreatedByUserId = authorId,
            PassingScorePercent = 60,
            TimeLimitMinutes = timeLimitMinutes,
            IsPublished = true,
            IsActive = true,
            TotalPoints = 15
        };

        var first = new QuizQuestion { QuestionText = "Q1", Points = 10, OrderIndex = 1 };
        first.Answers.Add(new QuizAnswer { AnswerText = "Right", IsCorrect = true, OrderIndex = 1 });
        first.Answers.Add(new QuizAnswer { AnswerText = "Wrong", IsCorrect = false, OrderIndex = 2 });

        var second = new QuizQuestion { QuestionText = "Q2", Points = 5, OrderIndex = 2 };
        second.Answers.Add(new QuizAnswer { AnswerText = "Right", IsCorrect = true, OrderIndex = 1 });
        second.Answers.Add(new QuizAnswer { AnswerText = "Wrong", IsCorrect = false, OrderIndex = 2 });

        quiz.Questions.Add(first);
        quiz.Questions.Add(second);
        db.Quizzes.Add(quiz);
        await db.SaveChangesAsync();
        return quiz;
    }

    public static async Task<Exercise> AddExerciseAsync(NeuroPathDbContext db, int points = 100, decimal multiplier = 1.0m, int durationMinutes = 10)
    {
        var exercise = new Exercise
        {
            Title = $"Exercise-{Guid.NewGuid():N}",
            Description = "Test exercise",
            Instructions = "Do it",
            DurationMinutes = durationMinutes,
            Points = points,
            IsActive = true,
            ExerciseCategory = new ExerciseCategory { Name = $"Category-{Guid.NewGuid():N}" },
            DifficultyLevel = new DifficultyLevel { Name = $"Level-{Guid.NewGuid():N}", Level = Random.Shared.Next(1000, 1_000_000), PointMultiplier = multiplier }
        };
        db.Exercises.Add(exercise);
        await db.SaveChangesAsync();
        return exercise;
    }

    public static int CorrectAnswerId(QuizQuestion question) => question.Answers.Single(a => a.IsCorrect).Id;

    public static int WrongAnswerId(QuizQuestion question) => question.Answers.Single(a => !a.IsCorrect).Id;
}
