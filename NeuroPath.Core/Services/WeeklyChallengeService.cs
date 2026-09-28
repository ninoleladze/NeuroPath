using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Challenges;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class WeeklyChallengeService : IWeeklyChallengeService
{
    private readonly NeuroPathDbContext _db;

    public WeeklyChallengeService(NeuroPathDbContext db) => _db = db;

    public async Task<List<WeeklyChallengeDto>> GetAllAsync()
        => await _db.WeeklyChallenges.Where(c => c.IsActive)
            .Include(c => c.UserChallenges)
            .Select(c => new WeeklyChallengeDto
            {
                Id = c.Id, Title = c.Title, Description = c.Description, StartDate = c.StartDate,
                EndDate = c.EndDate, TargetExerciseCount = c.TargetExerciseCount, TargetPoints = c.TargetPoints,
                BonusPoints = c.BonusPoints, IsActive = c.IsActive, IsTeamChallenge = c.IsTeamChallenge,
                ImageUrl = c.ImageUrl, ParticipantCount = c.UserChallenges.Count
            }).ToListAsync();

    public async Task<WeeklyChallengeDto?> GetByIdAsync(int id)
    {
        var c = await _db.WeeklyChallenges.Include(x => x.UserChallenges).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return null;
        return new WeeklyChallengeDto { Id = c.Id, Title = c.Title, Description = c.Description, StartDate = c.StartDate, EndDate = c.EndDate, TargetExerciseCount = c.TargetExerciseCount, TargetPoints = c.TargetPoints, BonusPoints = c.BonusPoints, IsActive = c.IsActive, IsTeamChallenge = c.IsTeamChallenge, ImageUrl = c.ImageUrl, ParticipantCount = c.UserChallenges.Count };
    }

    public async Task<WeeklyChallengeDto> CreateAsync(CreateWeeklyChallengeDto dto)
    {
        var c = new WeeklyChallenge { Title = dto.Title, Description = dto.Description, StartDate = dto.StartDate, EndDate = dto.EndDate, TargetExerciseCount = dto.TargetExerciseCount, TargetPoints = dto.TargetPoints, TargetAccuracy = dto.TargetAccuracy, BonusPoints = dto.BonusPoints, IsTeamChallenge = dto.IsTeamChallenge, ImageUrl = dto.ImageUrl };
        _db.WeeklyChallenges.Add(c);
        await _db.SaveChangesAsync();
        return new WeeklyChallengeDto { Id = c.Id, Title = c.Title, Description = c.Description, StartDate = c.StartDate, EndDate = c.EndDate, TargetExerciseCount = c.TargetExerciseCount, TargetPoints = c.TargetPoints, BonusPoints = c.BonusPoints, IsActive = c.IsActive, IsTeamChallenge = c.IsTeamChallenge, ImageUrl = c.ImageUrl };
    }

    public async Task<WeeklyChallengeDto?> UpdateAsync(int id, CreateWeeklyChallengeDto dto)
    {
        var c = await _db.WeeklyChallenges.FindAsync(id);
        if (c == null) return null;
        c.Title = dto.Title; c.Description = dto.Description; c.StartDate = dto.StartDate; c.EndDate = dto.EndDate;
        c.TargetExerciseCount = dto.TargetExerciseCount; c.TargetPoints = dto.TargetPoints;
        c.BonusPoints = dto.BonusPoints; c.IsTeamChallenge = dto.IsTeamChallenge; c.ImageUrl = dto.ImageUrl;
        await _db.SaveChangesAsync();
        return new WeeklyChallengeDto { Id = c.Id, Title = c.Title, Description = c.Description, StartDate = c.StartDate, EndDate = c.EndDate, TargetExerciseCount = c.TargetExerciseCount, TargetPoints = c.TargetPoints, BonusPoints = c.BonusPoints, IsActive = c.IsActive, IsTeamChallenge = c.IsTeamChallenge };
    }

    public async Task<bool> JoinAsync(int challengeId, string userId)
    {
        if (await _db.UserChallenges.AnyAsync(x => x.UserId == userId && x.WeeklyChallengeId == challengeId)) return false;
        _db.UserChallenges.Add(new UserChallenge { UserId = userId, WeeklyChallengeId = challengeId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> JoinAsTeamAsync(int challengeId, int teamId)
    {
        if (await _db.TeamChallengeEntries.AnyAsync(x => x.TeamId == teamId && x.WeeklyChallengeId == challengeId)) return false;
        _db.TeamChallengeEntries.Add(new TeamChallengeEntry { TeamId = teamId, WeeklyChallengeId = challengeId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProgressAsync(int challengeId, string userId, int points, int exercises, decimal accuracy)
    {
        var uc = await _db.UserChallenges.FirstOrDefaultAsync(x => x.UserId == userId && x.WeeklyChallengeId == challengeId);
        if (uc == null) return false;
        uc.EarnedPoints += points; uc.CompletedExercises += exercises; uc.CurrentAccuracy = accuracy;
        var challenge = await _db.WeeklyChallenges.FindAsync(challengeId);
        if (challenge != null && uc.EarnedPoints >= challenge.TargetPoints && uc.CompletedExercises >= challenge.TargetExerciseCount)
        {
            uc.Status = ChallengeStatus.Completed; uc.CompletedAt = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<object>> GetLeaderboardAsync(int challengeId)
    {
        var entries = await _db.UserChallenges.Where(uc => uc.WeeklyChallengeId == challengeId)
            .Include(uc => uc.User).OrderByDescending(uc => uc.EarnedPoints).Take(50).ToListAsync();
        return entries.Select((uc, i) => (object)new { rank = i + 1, userId = uc.UserId, userName = uc.User.UserName, earnedPoints = uc.EarnedPoints, completedExercises = uc.CompletedExercises, status = uc.Status.ToString() }).ToList();
    }
}
