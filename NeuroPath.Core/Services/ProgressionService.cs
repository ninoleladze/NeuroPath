using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Wellness;

namespace NeuroPath.Core.Services;

public class ProgressionService : IProgressionService
{
    private const int ExperiencePerLevel = 500;

    private readonly INeuroPathDbContext _db;

    public ProgressionService(INeuroPathDbContext db) => _db = db;

    public async Task AwardPointsAsync(string userId, int amount, PointSource source, int? referenceId = null)
    {
        if (amount <= 0) return;

        await _db.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.TotalPoints, u => u.TotalPoints + amount)
                .SetProperty(u => u.CurrentBalance, u => u.CurrentBalance + amount));

        _db.PointTransactions.Add(new PointTransaction
        {
            UserId = userId,
            Amount = amount,
            Source = source,
            ReferenceId = referenceId
        });
    }

    public async Task RegisterActivityAsync(string userId)
    {
        var today = DateTime.UtcNow.Date;
        var streak = await _db.Streaks.FirstOrDefaultAsync(s => s.UserId == userId);

        if (streak == null)
        {
            _db.Streaks.Add(new Streak { UserId = userId, CurrentStreak = 1, LongestStreak = 1, TotalActiveDays = 1, LastActivityDate = DateTime.UtcNow });
            return;
        }

        var lastDay = streak.LastActivityDate.Date;

        if (streak.CurrentStreak == 0)
        {
            streak.CurrentStreak = 1;
            streak.TotalActiveDays++;
        }
        else if (lastDay == today)
        {
            return;
        }
        else if (lastDay == today.AddDays(-1))
        {
            streak.CurrentStreak++;
            streak.TotalActiveDays++;
        }
        else if (lastDay == today.AddDays(-2) && streak.StreakFreezeCount > 0)
        {
            streak.StreakFreezeCount--;
            streak.CurrentStreak++;
            streak.TotalActiveDays++;
        }
        else
        {
            streak.CurrentStreak = 1;
            streak.TotalActiveDays++;
        }

        streak.LongestStreak = Math.Max(streak.LongestStreak, streak.CurrentStreak);
        streak.LastActivityDate = DateTime.UtcNow;
    }

    public async Task ApplyBrainAreaProgressAsync(string userId, Exercise exercise, int points, decimal accuracy)
    {
        var links = await _db.ExerciseBrainAreas
            .Where(x => x.ExerciseId == exercise.Id)
            .Select(x => new { x.BrainAreaId, x.IsPrimary })
            .ToListAsync();

        foreach (var link in links)
        {
            var progress = await _db.UserBrainAreas.FindAsync(userId, link.BrainAreaId);
            if (progress == null)
            {
                progress = new UserBrainArea { UserId = userId, BrainAreaId = link.BrainAreaId };
                _db.UserBrainAreas.Add(progress);
            }

            var experience = link.IsPrimary ? points : points / 2;
            progress.AverageAccuracy = ((progress.AverageAccuracy * progress.TotalExercises) + accuracy) / (progress.TotalExercises + 1);
            progress.TotalExercises++;
            progress.ExperiencePoints += experience;
            progress.CurrentLevel = 1 + progress.ExperiencePoints / ExperiencePerLevel;
            progress.LastTrainedAt = DateTime.UtcNow;
        }
    }

    public async Task ApplyChallengeProgressAsync(string userId, int points, bool countsAsExercise, decimal? accuracy)
    {
        var now = DateTime.UtcNow;

        var userChallenges = await _db.UserChallenges
            .Include(uc => uc.WeeklyChallenge)
            .Where(uc => uc.UserId == userId
                && uc.Status != ChallengeStatus.Completed
                && uc.WeeklyChallenge.IsActive
                && uc.WeeklyChallenge.StartDate <= now
                && uc.WeeklyChallenge.EndDate >= now)
            .ToListAsync();

        foreach (var uc in userChallenges)
        {
            uc.EarnedPoints += points;
            if (countsAsExercise)
            {
                uc.CompletedExercises++;
                if (accuracy.HasValue)
                    uc.CurrentAccuracy = ((uc.CurrentAccuracy * (uc.CompletedExercises - 1)) + accuracy.Value) / uc.CompletedExercises;
            }
            uc.Status = ChallengeStatus.InProgress;

            var challenge = uc.WeeklyChallenge;
            var reachedTargets = uc.EarnedPoints >= challenge.TargetPoints
                && uc.CompletedExercises >= challenge.TargetExerciseCount
                && (!challenge.TargetAccuracy.HasValue || uc.CurrentAccuracy >= challenge.TargetAccuracy.Value);

            if (reachedTargets)
            {
                uc.Status = ChallengeStatus.Completed;
                uc.CompletedAt = now;
                await AwardPointsAsync(userId, challenge.BonusPoints, PointSource.Challenge, challenge.Id);
            }
        }

        if (points <= 0) return;

        var memberships = await _db.TeamMembers
            .Include(m => m.Team)
            .Where(m => m.UserId == userId && m.Team.IsActive)
            .ToListAsync();

        if (memberships.Count == 0) return;

        var teamIds = memberships.Select(m => m.TeamId).ToList();
        foreach (var membership in memberships)
        {
            membership.ContributedPoints += points;
            membership.Team.TotalPoints += points;
        }

        var teamEntries = await _db.TeamChallengeEntries
            .Include(e => e.WeeklyChallenge)
            .Where(e => teamIds.Contains(e.TeamId)
                && e.Status != ChallengeStatus.Completed
                && e.WeeklyChallenge.IsActive
                && e.WeeklyChallenge.StartDate <= now
                && e.WeeklyChallenge.EndDate >= now)
            .ToListAsync();

        foreach (var entry in teamEntries)
        {
            entry.TotalPoints += points;
            entry.Status = ChallengeStatus.InProgress;
            if (entry.TotalPoints >= entry.WeeklyChallenge.TargetPoints)
            {
                entry.Status = ChallengeStatus.Completed;
                entry.CompletedAt = now;
            }
        }
    }
}
