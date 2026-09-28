using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class AchievementService : IAchievementService
{
    private readonly NeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public AchievementService(NeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<AchievementDto>> GetAllAsync()
    {
        const string key = "achievements";
        if (_cache.TryGet<List<AchievementDto>>(key, out var cached) && cached != null) return cached;
        var list = await _db.Achievements.Select(a => new AchievementDto { Id = a.Id, Name = a.Name, Description = a.Description, IconUrl = a.IconUrl, Tier = a.Tier, BonusPoints = a.BonusPoints, IsSecret = a.IsSecret, PointsRequired = a.PointsRequired, ExercisesRequired = a.ExercisesRequired, StreakDaysRequired = a.StreakDaysRequired }).ToListAsync();
        _cache.Set(key, list, TimeSpan.FromMinutes(30));
        return list;
    }

    public async Task<List<UserAchievementDto>> GetUserAchievementsAsync(string userId)
        => await _db.UserAchievements.Where(ua => ua.UserId == userId)
            .Include(ua => ua.Achievement)
            .Select(ua => new UserAchievementDto { AchievementId = ua.AchievementId, AchievementName = ua.Achievement.Name, Tier = ua.Achievement.Tier, IconUrl = ua.Achievement.IconUrl, EarnedAt = ua.EarnedAt })
            .ToListAsync();

    public async Task<AchievementDto> CreateAsync(AchievementDto dto)
    {
        var a = new Achievement { Name = dto.Name, Description = dto.Description, IconUrl = dto.IconUrl, Tier = dto.Tier, BonusPoints = dto.BonusPoints, IsSecret = dto.IsSecret, PointsRequired = dto.PointsRequired, ExercisesRequired = dto.ExercisesRequired, StreakDaysRequired = dto.StreakDaysRequired };
        _db.Achievements.Add(a);
        await _db.SaveChangesAsync();
        _cache.Remove("achievements");
        return new AchievementDto { Id = a.Id, Name = a.Name, Description = a.Description, Tier = a.Tier, BonusPoints = a.BonusPoints };
    }

    public async Task<AchievementDto?> UpdateAsync(int id, AchievementDto dto)
    {
        var a = await _db.Achievements.FindAsync(id);
        if (a == null) return null;
        a.Name = dto.Name; a.Description = dto.Description; a.Tier = dto.Tier; a.BonusPoints = dto.BonusPoints;
        a.PointsRequired = dto.PointsRequired; a.ExercisesRequired = dto.ExercisesRequired;
        a.StreakDaysRequired = dto.StreakDaysRequired; a.IsSecret = dto.IsSecret;
        await _db.SaveChangesAsync();
        _cache.Remove("achievements");
        return new AchievementDto { Id = a.Id, Name = a.Name, Description = a.Description, Tier = a.Tier, BonusPoints = a.BonusPoints };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var a = await _db.Achievements.FindAsync(id);
        if (a == null) return false;
        _db.Achievements.Remove(a);
        await _db.SaveChangesAsync();
        _cache.Remove("achievements");
        return true;
    }

    public async Task CheckAndAwardAsync(string userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return;
        var earned = await _db.UserAchievements.Where(ua => ua.UserId == userId).Select(ua => ua.AchievementId).ToListAsync();
        var achievements = await _db.Achievements.ToListAsync();
        var streak = await _db.Streaks.FirstOrDefaultAsync(s => s.UserId == userId);
        var exerciseCount = await _db.UserExerciseLogs.CountAsync(l => l.UserId == userId);
        var quizCount = await _db.UserQuizAttempts.CountAsync(a => a.UserId == userId && a.IsPassed);

        foreach (var ach in achievements.Where(a => !earned.Contains(a.Id)))
        {
            var qualify = true;
            if (ach.PointsRequired.HasValue && user.TotalPoints < ach.PointsRequired) qualify = false;
            if (ach.ExercisesRequired.HasValue && exerciseCount < ach.ExercisesRequired) qualify = false;
            if (ach.StreakDaysRequired.HasValue && (streak?.CurrentStreak ?? 0) < ach.StreakDaysRequired) qualify = false;
            if (ach.QuizzesRequired.HasValue && quizCount < ach.QuizzesRequired) qualify = false;
            if (qualify)
            {
                _db.UserAchievements.Add(new UserAchievement { UserId = userId, AchievementId = ach.Id });
                user.TotalPoints += ach.BonusPoints;
            }
        }
        await _db.SaveChangesAsync();
    }
}
