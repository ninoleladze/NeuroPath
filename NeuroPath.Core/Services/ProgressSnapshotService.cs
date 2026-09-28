using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Core.Models.DTOs.Progress;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class ProgressSnapshotService : IProgressSnapshotService
{
    private readonly NeuroPathDbContext _db;

    public ProgressSnapshotService(NeuroPathDbContext db) => _db = db;

    public async Task<List<ProgressSnapshotDto>> GetSnapshotsAsync(string userId)
        => await _db.ProgressSnapshots.Where(p => p.UserId == userId)
            .OrderByDescending(p => p.SnapshotDate)
            .Select(p => new ProgressSnapshotDto
            {
                Id = p.Id, SnapshotDate = p.SnapshotDate, TotalPoints = p.TotalPoints,
                ExercisesCompleted = p.ExercisesCompleted, QuizzesCompleted = p.QuizzesCompleted,
                CourseLessonsCompleted = p.CourseLessonsCompleted, AverageAccuracy = p.AverageAccuracy,
                TotalTrainingMinutes = p.TotalTrainingMinutes, StreakDays = p.StreakDays,
                TopBrainArea = p.TopBrainArea, WeakestBrainArea = p.WeakestBrainArea
            }).ToListAsync();

    public async Task<ProgressSummaryDto> GetSummaryAsync(string userId)
    {
        var user = await _db.Users.FindAsync(userId);
        var streak = await _db.Streaks.FirstOrDefaultAsync(s => s.UserId == userId);
        var exerciseCount = await _db.UserExerciseLogs.CountAsync(l => l.UserId == userId);
        var quizCount = await _db.UserQuizAttempts.CountAsync(a => a.UserId == userId && a.CompletedAt.HasValue);
        var trainingMinutes = await _db.UserExerciseLogs.Where(l => l.UserId == userId).SumAsync(l => (int?)l.DurationSeconds) ?? 0;
        var avgAccuracy = await _db.UserExerciseLogs.Where(l => l.UserId == userId).AverageAsync(l => (decimal?)l.AccuracyPercent) ?? 0;

        var topBrainArea = await _db.UserBrainAreas.Where(uba => uba.UserId == userId)
            .Include(uba => uba.BrainArea).OrderByDescending(uba => uba.ExperiencePoints)
            .Select(uba => uba.BrainArea.Name).FirstOrDefaultAsync();
        var weakestBrainArea = await _db.UserBrainAreas.Where(uba => uba.UserId == userId)
            .Include(uba => uba.BrainArea).OrderBy(uba => uba.ExperiencePoints)
            .Select(uba => uba.BrainArea.Name).FirstOrDefaultAsync();

        return new ProgressSummaryDto
        {
            TotalPoints = user?.TotalPoints ?? 0,
            ExercisesCompleted = exerciseCount,
            QuizzesCompleted = quizCount,
            StreakDays = streak?.CurrentStreak ?? 0,
            AverageAccuracy = avgAccuracy,
            TotalTrainingMinutes = trainingMinutes / 60,
            TopBrainArea = topBrainArea,
            WeakestBrainArea = weakestBrainArea
        };
    }

    public async Task<List<BrainAreaBreakdownDto>> GetBrainAreaBreakdownAsync(string userId)
        => await _db.UserBrainAreas.Where(uba => uba.UserId == userId)
            .Include(uba => uba.BrainArea)
            .Select(uba => new BrainAreaBreakdownDto
            {
                BrainAreaId = uba.BrainAreaId, BrainAreaName = uba.BrainArea.Name,
                Color = uba.BrainArea.Color, CurrentLevel = uba.CurrentLevel,
                ExperiencePoints = uba.ExperiencePoints, TotalExercises = uba.TotalExercises,
                AverageAccuracy = uba.AverageAccuracy
            }).ToListAsync();

    public async Task TakeSnapshotAsync(string userId)
    {
        var summary = await GetSummaryAsync(userId);
        var snapshot = new ProgressSnapshot
        {
            UserId = userId, SnapshotDate = DateTime.UtcNow,
            TotalPoints = summary.TotalPoints, ExercisesCompleted = summary.ExercisesCompleted,
            QuizzesCompleted = summary.QuizzesCompleted, AverageAccuracy = summary.AverageAccuracy,
            TotalTrainingMinutes = summary.TotalTrainingMinutes, StreakDays = summary.StreakDays,
            TopBrainArea = summary.TopBrainArea, WeakestBrainArea = summary.WeakestBrainArea
        };
        _db.ProgressSnapshots.Add(snapshot);
        await _db.SaveChangesAsync();
    }
}
