using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Core.Models.DTOs.Training;
using NeuroPath.Core.Settings;

namespace NeuroPath.Core.Services;

public class DailySessionService : IDailySessionService
{
    private readonly INeuroPathDbContext _db;
    private readonly IProgressionService _progression;
    private readonly IAchievementService _achievements;
    private readonly GamificationSettings _rules;

    public DailySessionService(INeuroPathDbContext db, IProgressionService progression, IAchievementService achievements, IOptions<GamificationSettings> rules)
    {
        _db = db;
        _progression = progression;
        _achievements = achievements;
        _rules = rules.Value;
    }

    public async Task<List<DailySessionDto>> GetUserSessionsAsync(string userId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.DailySessions.AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SessionDate).ThenByDescending(s => s.StartTime)
            .Skip(skip).Take(take)
            .Select(s => new DailySessionDto
            {
                Id = s.Id,
                SessionDate = s.SessionDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                TotalPoints = s.TotalPoints,
                TotalExercises = s.TotalExercises,
                Status = s.Status,
                Notes = s.Notes
            })
            .ToListAsync();
    }

    public async Task<DailySessionDto> StartSessionAsync(CreateDailySessionDto dto, string userId)
    {
        if (dto.TrainingPlanId.HasValue
            && !await _db.TrainingPlans.AnyAsync(p => p.Id == dto.TrainingPlanId && p.UserId == userId && p.IsActive))
            throw new NotFoundException("Training plan not found.");

        var openSessionId = await _db.DailySessions
            .Where(s => s.UserId == userId && s.Status == SessionStatus.InProgress)
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync();
        if (openSessionId.HasValue)
            throw new ConflictException($"Session {openSessionId} is still in progress. Complete it before starting a new one.");

        var session = new DailySession
        {
            UserId = userId,
            TrainingPlanId = dto.TrainingPlanId,
            SessionDate = dto.SessionDate.Date,
            Notes = dto.Notes
        };
        _db.DailySessions.Add(session);
        await _db.SaveChangesAsync();
        return ToDto(session);
    }

    public async Task<DailySessionDto?> CompleteSessionAsync(int sessionId, string userId)
    {
        var session = await _db.DailySessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
        if (session == null) return null;
        if (session.Status == SessionStatus.InProgress)
        {
            session.Status = session.TotalExercises > 0 ? SessionStatus.Completed : SessionStatus.Abandoned;
            session.EndTime = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        return ToDto(session);
    }

    public async Task<ExerciseLogResultDto?> AddExerciseLogAsync(int sessionId, CreateUserExerciseLogDto dto, string userId)
    {
        var session = await _db.DailySessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
        if (session == null) return null;
        if (session.Status != SessionStatus.InProgress)
            throw new BusinessRuleException("Exercises can only be logged to a session that is in progress.");

        var exercise = await _db.Exercises.AsNoTracking()
            .Include(e => e.DifficultyLevel)
            .Include(e => e.Hints)
            .FirstOrDefaultAsync(e => e.Id == dto.ExerciseId && e.IsActive)
            ?? throw new NotFoundException("Exercise not found.");

        var minimumSeconds = (int)Math.Floor(exercise.DurationMinutes * 60 * _rules.MinimumDurationFraction);
        if (dto.DurationSeconds < minimumSeconds)
            throw new BusinessRuleException($"This exercise cannot be completed in under {minimumSeconds} seconds.");

        var now = DateTime.UtcNow;
        var accuracy = Math.Clamp(dto.AccuracyPercent, 0m, 100m);
        var hintPenalty = exercise.Hints.OrderBy(h => h.HintOrder).Take(Math.Max(0, dto.HintsUsed)).Sum(h => h.PointPenalty);
        var basePoints = (int)Math.Round(exercise.Points * exercise.DifficultyLevel.PointMultiplier * accuracy / 100m, MidpointRounding.AwayFromZero);
        var points = await ApplyDailyLimitsAsync(userId, exercise.Id, Math.Max(0, basePoints - hintPenalty), now);

        var log = new UserExerciseLog
        {
            UserId = userId,
            ExerciseId = exercise.Id,
            CompletedAt = now,
            Score = dto.Score,
            DurationSeconds = dto.DurationSeconds,
            AccuracyPercent = accuracy,
            DifficultyAtCompletion = exercise.DifficultyLevel.Level,
            PointsAwarded = points
        };

        await using (var transaction = await _db.Database.BeginTransactionAsync())
        {
            _db.UserExerciseLogs.Add(log);
            _db.SessionExercises.Add(new SessionExercise
            {
                DailySessionId = session.Id,
                ExerciseId = exercise.Id,
                StartedAt = now.AddSeconds(-Math.Max(0, dto.DurationSeconds)),
                CompletedAt = now,
                Score = dto.Score,
                MaxScore = exercise.Points,
                AccuracyPercent = accuracy,
                HintsUsed = Math.Max(0, dto.HintsUsed),
                OrderInSession = session.TotalExercises + 1
            });

            session.TotalExercises++;
            session.TotalPoints += points;

            if (session.TrainingPlanId.HasValue)
            {
                var planItem = await _db.TrainingPlanExercises
                    .Where(x => x.TrainingPlanId == session.TrainingPlanId && x.ExerciseId == exercise.Id && !x.IsCompleted)
                    .OrderBy(x => x.DayOfWeek).ThenBy(x => x.OrderInDay)
                    .FirstOrDefaultAsync();
                if (planItem != null) planItem.IsCompleted = true;
            }

            await _db.Exercises.Where(e => e.Id == exercise.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.TotalCompletions, e => e.TotalCompletions + 1));

            await _progression.AwardPointsAsync(userId, points, PointSource.Exercise, exercise.Id);
            await _progression.RegisterActivityAsync(userId);
            await _progression.ApplyBrainAreaProgressAsync(userId, exercise, points, accuracy);
            await _progression.ApplyChallengeProgressAsync(userId, points, countsAsExercise: true, accuracy);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        await _achievements.CheckAndAwardAsync(userId);

        return new ExerciseLogResultDto
        {
            LogId = log.Id,
            PointsAwarded = points,
            SessionTotalPoints = session.TotalPoints,
            SessionTotalExercises = session.TotalExercises
        };
    }

    private async Task<int> ApplyDailyLimitsAsync(string userId, int exerciseId, int points, DateTime now)
    {
        if (points <= 0) return 0;

        var zone = await _progression.GetTimeZoneAsync(userId);
        var dayStart = UserTime.StartOfLocalDayUtc(now, zone);

        var today = await _db.UserExerciseLogs
            .Where(l => l.UserId == userId && l.CompletedAt >= dayStart)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                ScoredRepeats = g.Count(l => l.ExerciseId == exerciseId && l.PointsAwarded > 0),
                Points = g.Sum(l => l.PointsAwarded)
            })
            .FirstOrDefaultAsync();

        if (today == null) return Math.Min(points, _rules.DailyExercisePointCap);
        if (today.ScoredRepeats >= _rules.MaxScoredRepeatsPerExercisePerDay) return 0;
        return Math.Clamp(_rules.DailyExercisePointCap - today.Points, 0, points);
    }

    public async Task<List<UserExerciseLogDto>> GetUserLogsAsync(string userId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.UserExerciseLogs.AsNoTracking()
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CompletedAt)
            .Skip(skip).Take(take)
            .Select(l => new UserExerciseLogDto
            {
                Id = l.Id,
                ExerciseId = l.ExerciseId,
                ExerciseTitle = l.Exercise.Title,
                CompletedAt = l.CompletedAt,
                Score = l.Score,
                DurationSeconds = l.DurationSeconds,
                AccuracyPercent = l.AccuracyPercent,
                DifficultyAtCompletion = l.DifficultyAtCompletion
            })
            .ToListAsync();
    }

    public async Task<SessionStatsDto> GetStatsAsync(string userId)
    {
        var stats = await _db.UserExerciseLogs.AsNoTracking()
            .Where(l => l.UserId == userId)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Count = g.Count(),
                AverageAccuracy = g.Average(l => l.AccuracyPercent),
                TotalSeconds = g.Sum(l => l.DurationSeconds)
            })
            .FirstOrDefaultAsync();

        return new SessionStatsDto
        {
            TotalExercisesCompleted = stats?.Count ?? 0,
            AverageAccuracy = stats == null ? 0 : Math.Round(stats.AverageAccuracy, 2),
            TotalTrainingMinutes = (stats?.TotalSeconds ?? 0) / 60
        };
    }

    private static DailySessionDto ToDto(DailySession s) => new()
    {
        Id = s.Id,
        SessionDate = s.SessionDate,
        StartTime = s.StartTime,
        EndTime = s.EndTime,
        TotalPoints = s.TotalPoints,
        TotalExercises = s.TotalExercises,
        Status = s.Status,
        Notes = s.Notes
    };
}
