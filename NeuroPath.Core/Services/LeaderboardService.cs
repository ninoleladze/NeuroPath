using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly NeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public LeaderboardService(NeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(string period = "weekly")
    {
        var key = $"leaderboard-{period}";
        if (_cache.TryGet<List<LeaderboardEntryDto>>(key, out var cached) && cached != null) return cached;

        var list = await _db.LeaderboardEntries.Where(e => e.Period == period)
            .Include(e => e.User)
            .OrderBy(e => e.Rank)
            .Take(100)
            .Select(e => new LeaderboardEntryDto
            {
                Rank = e.Rank, UserId = e.UserId, UserName = e.User.UserName!,
                AvatarUrl = e.User.AvatarUrl, TotalPoints = e.TotalPoints,
                ExercisesCompleted = e.ExercisesCompleted, QuizzesCompleted = e.QuizzesCompleted
            }).ToListAsync();

        if (!list.Any())
        {
            // Generate from user data if no leaderboard entries exist
            var cutoff = period switch
            {
                "weekly" => DateTime.UtcNow.AddDays(-7),
                "monthly" => DateTime.UtcNow.AddDays(-30),
                _ => DateTime.MinValue
            };
            var users = await _db.Users.Where(u => u.IsActive && !u.IsBlocked)
                .OrderByDescending(u => u.TotalPoints)
                .Take(100)
                .Select(u => new { u.Id, u.UserName, u.AvatarUrl, u.TotalPoints })
                .ToListAsync();
            list = users.Select((u, i) => new LeaderboardEntryDto
            {
                Rank = i + 1, UserId = u.Id, UserName = u.UserName!, AvatarUrl = u.AvatarUrl, TotalPoints = u.TotalPoints
            }).ToList();
        }

        _cache.Set(key, list, TimeSpan.FromMinutes(5));
        return list;
    }

    public async Task RefreshLeaderboardAsync(string period)
    {
        var cutoff = period switch
        {
            "weekly" => DateTime.UtcNow.AddDays(-7),
            "monthly" => DateTime.UtcNow.AddDays(-30),
            _ => DateTime.MinValue
        };

        var existing = await _db.LeaderboardEntries.Where(e => e.Period == period).ToListAsync();
        _db.LeaderboardEntries.RemoveRange(existing);

        var users = await _db.Users.Where(u => u.IsActive && !u.IsBlocked)
            .OrderByDescending(u => u.TotalPoints).Take(100).ToListAsync();

        for (int i = 0; i < users.Count; i++)
        {
            var u = users[i];
            var exerciseCount = await _db.UserExerciseLogs.CountAsync(l => l.UserId == u.Id && l.CompletedAt >= cutoff);
            var quizCount = await _db.UserQuizAttempts.CountAsync(a => a.UserId == u.Id && a.CompletedAt >= cutoff);
            _db.LeaderboardEntries.Add(new LeaderboardEntry
            {
                UserId = u.Id, Period = period, TotalPoints = u.TotalPoints,
                Rank = i + 1, ExercisesCompleted = exerciseCount, QuizzesCompleted = quizCount
            });
        }
        await _db.SaveChangesAsync();
        _cache.Remove($"leaderboard-{period}");
    }
}
