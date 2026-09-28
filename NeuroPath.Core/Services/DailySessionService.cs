using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Core.Models.DTOs.Training;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class DailySessionService : IDailySessionService
{
    private readonly NeuroPathDbContext _db;

    public DailySessionService(NeuroPathDbContext db) => _db = db;

    public async Task<List<DailySessionDto>> GetUserSessionsAsync(string userId)
        => await _db.DailySessions.Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SessionDate)
            .Select(s => new DailySessionDto { Id = s.Id, SessionDate = s.SessionDate, StartTime = s.StartTime, EndTime = s.EndTime, TotalPoints = s.TotalPoints, TotalExercises = s.TotalExercises, Status = s.Status, Notes = s.Notes })
            .ToListAsync();

    public async Task<DailySessionDto> StartSessionAsync(CreateDailySessionDto dto, string userId)
    {
        var s = new DailySession { UserId = userId, TrainingPlanId = dto.TrainingPlanId, SessionDate = dto.SessionDate, Notes = dto.Notes };
        _db.DailySessions.Add(s);
        await _db.SaveChangesAsync();
        return new DailySessionDto { Id = s.Id, SessionDate = s.SessionDate, StartTime = s.StartTime, Status = s.Status, Notes = s.Notes };
    }

    public async Task<DailySessionDto?> CompleteSessionAsync(int sessionId, string userId)
    {
        var s = await _db.DailySessions.FirstOrDefaultAsync(x => x.Id == sessionId && x.UserId == userId);
        if (s == null) return null;
        s.Status = SessionStatus.Completed; s.EndTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new DailySessionDto { Id = s.Id, SessionDate = s.SessionDate, StartTime = s.StartTime, EndTime = s.EndTime, TotalPoints = s.TotalPoints, TotalExercises = s.TotalExercises, Status = s.Status, Notes = s.Notes };
    }

    public async Task<bool> AddExerciseLogAsync(int sessionId, CreateUserExerciseLogDto dto, string userId)
    {
        var session = await _db.DailySessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
        if (session == null) return false;

        var log = new UserExerciseLog { UserId = userId, ExerciseId = dto.ExerciseId, Score = dto.Score, DurationSeconds = dto.DurationSeconds, AccuracyPercent = dto.AccuracyPercent, DifficultyAtCompletion = dto.DifficultyAtCompletion };
        _db.UserExerciseLogs.Add(log);

        var exercise = await _db.Exercises.FindAsync(dto.ExerciseId);
        session.TotalExercises++;
        session.TotalPoints += exercise?.Points ?? 0;

        var user = await _db.Users.FindAsync(userId);
        if (user != null) { user.TotalPoints += exercise?.Points ?? 0; }

        if (exercise != null) exercise.TotalCompletions++;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserExerciseLogDto>> GetUserLogsAsync(string userId)
        => await _db.UserExerciseLogs.Where(l => l.UserId == userId)
            .Include(l => l.Exercise)
            .OrderByDescending(l => l.CompletedAt)
            .Select(l => new UserExerciseLogDto { Id = l.Id, ExerciseId = l.ExerciseId, ExerciseTitle = l.Exercise.Title, CompletedAt = l.CompletedAt, Score = l.Score, DurationSeconds = l.DurationSeconds, AccuracyPercent = l.AccuracyPercent, DifficultyAtCompletion = l.DifficultyAtCompletion })
            .ToListAsync();

    public async Task<object> GetStatsAsync(string userId)
    {
        var totalLogs = await _db.UserExerciseLogs.CountAsync(l => l.UserId == userId);
        var avgAccuracy = await _db.UserExerciseLogs.Where(l => l.UserId == userId).AverageAsync(l => (decimal?)l.AccuracyPercent) ?? 0;
        var totalMinutes = await _db.UserExerciseLogs.Where(l => l.UserId == userId).SumAsync(l => (int?)l.DurationSeconds) ?? 0;
        return new { totalExercisesCompleted = totalLogs, averageAccuracy = avgAccuracy, totalTrainingMinutes = totalMinutes / 60 };
    }
}
