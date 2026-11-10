using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Core.Models.DTOs.Progress;

namespace NeuroPath.Core.Services;

public class ProgressSnapshotService : IProgressSnapshotService
{
    private readonly INeuroPathDbContext _db;

    public ProgressSnapshotService(INeuroPathDbContext db) => _db = db;

    public async Task<List<ProgressSnapshotDto>> GetSnapshotsAsync(string userId, int page = 1, int pageSize = 30)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.ProgressSnapshots.AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.SnapshotDate)
            .Skip(skip).Take(take)
            .Select(p => new ProgressSnapshotDto
            {
                Id = p.Id,
                SnapshotDate = p.SnapshotDate,
                TotalPoints = p.TotalPoints,
                ExercisesCompleted = p.ExercisesCompleted,
                QuizzesCompleted = p.QuizzesCompleted,
                CourseLessonsCompleted = p.CourseLessonsCompleted,
                AverageAccuracy = p.AverageAccuracy,
                TotalTrainingMinutes = p.TotalTrainingMinutes,
                StreakDays = p.StreakDays,
                TopBrainArea = p.TopBrainArea,
                WeakestBrainArea = p.WeakestBrainArea
            })
            .ToListAsync();
    }

    public async Task<ProgressSummaryDto> GetSummaryAsync(string userId)
    {
        var user = await _db.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.TotalPoints,
                Streak = u.Streak != null ? u.Streak.CurrentStreak : 0,
                Quizzes = u.QuizAttempts.Count(a => a.CompletedAt != null)
            })
            .FirstOrDefaultAsync();
        if (user == null) return new ProgressSummaryDto();

        var exerciseStats = await _db.UserExerciseLogs.AsNoTracking()
            .Where(l => l.UserId == userId)
            .GroupBy(_ => 1)
            .Select(g => new { Count = g.Count(), Seconds = g.Sum(l => l.DurationSeconds), Accuracy = g.Average(l => l.AccuracyPercent) })
            .FirstOrDefaultAsync();

        var lessons = await _db.UserLessonCompletions.CountAsync(c => c.UserId == userId);

        var brainAreas = await _db.UserBrainAreas.AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.ExperiencePoints)
            .Select(b => b.BrainArea.Name)
            .ToListAsync();

        return new ProgressSummaryDto
        {
            TotalPoints = user.TotalPoints,
            ExercisesCompleted = exerciseStats?.Count ?? 0,
            QuizzesCompleted = user.Quizzes,
            CourseLessonsCompleted = lessons,
            StreakDays = user.Streak,
            AverageAccuracy = exerciseStats == null ? 0 : Math.Round(exerciseStats.Accuracy, 2),
            TotalTrainingMinutes = (exerciseStats?.Seconds ?? 0) / 60,
            TopBrainArea = brainAreas.FirstOrDefault(),
            WeakestBrainArea = brainAreas.Count > 1 ? brainAreas[^1] : null
        };
    }

    public async Task<List<BrainAreaBreakdownDto>> GetBrainAreaBreakdownAsync(string userId)
        => await _db.UserBrainAreas.AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.BrainArea.DisplayOrder)
            .Select(b => new BrainAreaBreakdownDto
            {
                BrainAreaId = b.BrainAreaId,
                BrainAreaName = b.BrainArea.Name,
                Color = b.BrainArea.Color,
                CurrentLevel = b.CurrentLevel,
                ExperiencePoints = b.ExperiencePoints,
                TotalExercises = b.TotalExercises,
                AverageAccuracy = b.AverageAccuracy
            })
            .ToListAsync();

    public async Task TakeSnapshotAsync(string userId)
    {
        var summary = await GetSummaryAsync(userId);
        var since = DateTime.UtcNow.AddDays(-7);
        var moods = await _db.MoodLogs.AsNoTracking()
            .Where(m => m.UserId == userId && m.LogDate >= since)
            .Select(m => (int)m.MoodBefore)
            .ToListAsync();

        _db.ProgressSnapshots.Add(new ProgressSnapshot
        {
            UserId = userId,
            SnapshotDate = DateTime.UtcNow,
            TotalPoints = summary.TotalPoints,
            ExercisesCompleted = summary.ExercisesCompleted,
            QuizzesCompleted = summary.QuizzesCompleted,
            CourseLessonsCompleted = summary.CourseLessonsCompleted,
            AverageAccuracy = summary.AverageAccuracy,
            TotalTrainingMinutes = summary.TotalTrainingMinutes,
            StreakDays = summary.StreakDays,
            TopBrainArea = summary.TopBrainArea,
            WeakestBrainArea = summary.WeakestBrainArea,
            MoodAverage = moods.Count == 0 ? null : Math.Round((decimal)moods.Average(), 2)
        });
        await _db.SaveChangesAsync();
    }
}
