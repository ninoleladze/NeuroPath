using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Services;

public class AchievementService : IAchievementService
{
    private const string CacheKey = "achievements";

    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;
    private readonly IProgressionService _progression;

    public AchievementService(INeuroPathDbContext db, ICacheService cache, IProgressionService progression)
    {
        _db = db;
        _cache = cache;
        _progression = progression;
    }

    public async Task<List<AchievementDto>> GetAllAsync(bool includeSecret = false)
    {
        if (!_cache.TryGet<List<AchievementDto>>(CacheKey, out var all) || all == null)
        {
            all = await _db.Achievements.AsNoTracking().Select(a => ToDto(a)).ToListAsync();
            _cache.Set(CacheKey, all, TimeSpan.FromMinutes(30));
        }
        return includeSecret ? all : all.Where(a => !a.IsSecret).ToList();
    }

    public async Task<List<UserAchievementDto>> GetUserAchievementsAsync(string userId)
        => await _db.UserAchievements.AsNoTracking()
            .Where(ua => ua.UserId == userId)
            .OrderByDescending(ua => ua.EarnedAt)
            .Select(ua => new UserAchievementDto
            {
                AchievementId = ua.AchievementId,
                AchievementName = ua.Achievement.Name,
                Tier = ua.Achievement.Tier,
                IconUrl = ua.Achievement.IconUrl,
                EarnedAt = ua.EarnedAt
            })
            .ToListAsync();

    public async Task<AchievementDto> CreateAsync(CreateAchievementDto dto)
    {
        var achievement = new Achievement();
        Apply(achievement, dto);
        _db.Achievements.Add(achievement);
        await _db.SaveChangesAsync();
        _cache.Remove(CacheKey);
        return ToDto(achievement);
    }

    public async Task<AchievementDto?> UpdateAsync(int id, CreateAchievementDto dto)
    {
        var achievement = await _db.Achievements.FindAsync(id);
        if (achievement == null) return null;
        Apply(achievement, dto);
        await _db.SaveChangesAsync();
        _cache.Remove(CacheKey);
        return ToDto(achievement);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var achievement = await _db.Achievements.FindAsync(id);
        if (achievement == null) return false;
        if (await _db.UserAchievements.AnyAsync(ua => ua.AchievementId == id))
            throw new ConflictException("This achievement has already been earned by users and cannot be deleted.");
        _db.Achievements.Remove(achievement);
        await _db.SaveChangesAsync();
        _cache.Remove(CacheKey);
        return true;
    }

    public async Task<List<UserAchievementDto>> CheckAndAwardAsync(string userId)
    {
        var earnedIds = await _db.UserAchievements.Where(ua => ua.UserId == userId).Select(ua => ua.AchievementId).ToListAsync();
        var candidates = await _db.Achievements.Where(a => !earnedIds.Contains(a.Id)).ToListAsync();
        if (candidates.Count == 0) return new List<UserAchievementDto>();

        var stats = await _db.Users
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.TotalPoints,
                Streak = u.Streak != null ? u.Streak.CurrentStreak : 0,
                Exercises = u.ExerciseLogs.Count(),
                Quizzes = u.QuizAttempts.Count(a => a.IsPassed),
                Courses = u.ModuleProgresses.Count(p => p.CompletedAt != null)
            })
            .FirstOrDefaultAsync();
        if (stats == null) return new List<UserAchievementDto>();

        var awarded = new List<UserAchievementDto>();
        await using var transaction = await _db.Database.BeginTransactionAsync();

        foreach (var achievement in candidates)
        {
            var qualifies =
                (!achievement.PointsRequired.HasValue || stats.TotalPoints >= achievement.PointsRequired) &&
                (!achievement.ExercisesRequired.HasValue || stats.Exercises >= achievement.ExercisesRequired) &&
                (!achievement.StreakDaysRequired.HasValue || stats.Streak >= achievement.StreakDaysRequired) &&
                (!achievement.QuizzesRequired.HasValue || stats.Quizzes >= achievement.QuizzesRequired) &&
                (!achievement.CoursesRequired.HasValue || stats.Courses >= achievement.CoursesRequired);

            if (!qualifies) continue;

            var earnedAt = DateTime.UtcNow;
            _db.UserAchievements.Add(new UserAchievement { UserId = userId, AchievementId = achievement.Id, EarnedAt = earnedAt });
            _db.Notifications.Add(new Notification
            {
                UserId = userId,
                Title = "Achievement unlocked",
                Message = $"You earned \"{achievement.Name}\" (+{achievement.BonusPoints} points).",
                Type = NotificationType.AchievementUnlocked,
                ReferenceId = achievement.Id,
                ReferenceType = nameof(Achievement)
            });
            await _progression.AwardPointsAsync(userId, achievement.BonusPoints, PointSource.Achievement, achievement.Id);

            awarded.Add(new UserAchievementDto
            {
                AchievementId = achievement.Id,
                AchievementName = achievement.Name,
                Tier = achievement.Tier,
                IconUrl = achievement.IconUrl,
                EarnedAt = earnedAt
            });
        }

        await _db.SaveChangesAsync();
        await transaction.CommitAsync();
        return awarded;
    }

    private static void Apply(Achievement a, CreateAchievementDto dto)
    {
        a.Name = dto.Name;
        a.Description = dto.Description;
        a.IconUrl = dto.IconUrl;
        a.Tier = dto.Tier;
        a.BonusPoints = dto.BonusPoints;
        a.IsSecret = dto.IsSecret;
        a.PointsRequired = dto.PointsRequired;
        a.ExercisesRequired = dto.ExercisesRequired;
        a.StreakDaysRequired = dto.StreakDaysRequired;
        a.QuizzesRequired = dto.QuizzesRequired;
        a.CoursesRequired = dto.CoursesRequired;
    }

    private static AchievementDto ToDto(Achievement a) => new()
    {
        Id = a.Id,
        Name = a.Name,
        Description = a.Description,
        IconUrl = a.IconUrl,
        Tier = a.Tier,
        BonusPoints = a.BonusPoints,
        IsSecret = a.IsSecret,
        PointsRequired = a.PointsRequired,
        ExercisesRequired = a.ExercisesRequired,
        StreakDaysRequired = a.StreakDaysRequired,
        QuizzesRequired = a.QuizzesRequired,
        CoursesRequired = a.CoursesRequired
    };
}
