using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Services;

public class LeaderboardService : ILeaderboardService
{
    private const int Size = 100;
    private static readonly string[] Periods = { "weekly", "monthly", "alltime" };

    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public LeaderboardService(INeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(string period = "weekly")
    {
        var normalized = Normalize(period);
        var key = $"leaderboard-{normalized}";
        if (_cache.TryGet<List<LeaderboardEntryDto>>(key, out var cached) && cached != null) return cached;

        var list = await ComputeAsync(normalized);
        _cache.Set(key, list, TimeSpan.FromMinutes(5));
        return list;
    }

    public async Task RefreshLeaderboardAsync(string period)
    {
        var normalized = Normalize(period);
        var entries = await ComputeAsync(normalized);

        await using var transaction = await _db.Database.BeginTransactionAsync();
        await _db.LeaderboardEntries.Where(e => e.Period == normalized).ExecuteDeleteAsync();
        _db.LeaderboardEntries.AddRange(entries.Select(e => new LeaderboardEntry
        {
            UserId = e.UserId,
            Period = normalized,
            TotalPoints = e.TotalPoints,
            Rank = e.Rank,
            ExercisesCompleted = e.ExercisesCompleted,
            QuizzesCompleted = e.QuizzesCompleted
        }));
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        _cache.Remove($"leaderboard-{normalized}");
    }

    private async Task<List<LeaderboardEntryDto>> ComputeAsync(string period)
    {
        var cutoff = period switch
        {
            "weekly" => DateTime.UtcNow.AddDays(-7),
            "monthly" => DateTime.UtcNow.AddDays(-30),
            _ => DateTime.MinValue
        };

        var visibleUsers = _db.Users.Where(u => u.IsActive && !u.IsBlocked && (u.UserSettings == null || u.UserSettings.ShowOnLeaderboard));

        var rows = period == "alltime"
            ? await visibleUsers
                .Where(u => u.TotalPoints > 0)
                .OrderByDescending(u => u.TotalPoints).ThenBy(u => u.UserName)
                .Take(Size)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.AvatarUrl,
                    Points = u.TotalPoints,
                    Exercises = u.ExerciseLogs.Count(),
                    Quizzes = u.QuizAttempts.Count(a => a.CompletedAt != null)
                })
                .ToListAsync()
            : await visibleUsers
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.AvatarUrl,
                    Points = _db.PointTransactions.Where(t => t.UserId == u.Id && t.CreatedAt >= cutoff).Sum(t => (int?)t.Amount) ?? 0,
                    Exercises = u.ExerciseLogs.Count(l => l.CompletedAt >= cutoff),
                    Quizzes = u.QuizAttempts.Count(a => a.CompletedAt >= cutoff)
                })
                .Where(x => x.Points > 0)
                .OrderByDescending(x => x.Points).ThenBy(x => x.UserName)
                .Take(Size)
                .ToListAsync();

        return rows.Select((r, i) => new LeaderboardEntryDto
        {
            Rank = i + 1,
            UserId = r.Id,
            UserName = r.UserName ?? string.Empty,
            AvatarUrl = r.AvatarUrl,
            TotalPoints = r.Points,
            ExercisesCompleted = r.Exercises,
            QuizzesCompleted = r.Quizzes
        }).ToList();
    }

    private static string Normalize(string period)
    {
        var value = (period ?? string.Empty).Trim().ToLowerInvariant();
        if (!Periods.Contains(value))
            throw new BusinessRuleException("Period must be one of: weekly, monthly, alltime.");
        return value;
    }
}
