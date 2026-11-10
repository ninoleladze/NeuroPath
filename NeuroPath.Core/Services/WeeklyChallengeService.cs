using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Challenges;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class WeeklyChallengeService : IWeeklyChallengeService
{
    private readonly INeuroPathDbContext _db;

    public WeeklyChallengeService(INeuroPathDbContext db) => _db = db;

    public async Task<List<WeeklyChallengeDto>> GetAllAsync()
    {
        var now = DateTime.UtcNow;
        return await _db.WeeklyChallenges.AsNoTracking()
            .Where(c => c.IsActive && c.EndDate >= now)
            .OrderBy(c => c.StartDate)
            .Select(c => new WeeklyChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TargetExerciseCount = c.TargetExerciseCount,
                TargetPoints = c.TargetPoints,
                BonusPoints = c.BonusPoints,
                IsActive = c.IsActive,
                IsTeamChallenge = c.IsTeamChallenge,
                ImageUrl = c.ImageUrl,
                ParticipantCount = c.IsTeamChallenge ? c.TeamChallengeEntries.Count : c.UserChallenges.Count
            })
            .ToListAsync();
    }

    public async Task<WeeklyChallengeDto?> GetByIdAsync(int id)
        => await _db.WeeklyChallenges.AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new WeeklyChallengeDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TargetExerciseCount = c.TargetExerciseCount,
                TargetPoints = c.TargetPoints,
                BonusPoints = c.BonusPoints,
                IsActive = c.IsActive,
                IsTeamChallenge = c.IsTeamChallenge,
                ImageUrl = c.ImageUrl,
                ParticipantCount = c.IsTeamChallenge ? c.TeamChallengeEntries.Count : c.UserChallenges.Count
            })
            .FirstOrDefaultAsync();

    public async Task<WeeklyChallengeDto> CreateAsync(CreateWeeklyChallengeDto dto)
    {
        var challenge = new WeeklyChallenge();
        Apply(challenge, dto);
        _db.WeeklyChallenges.Add(challenge);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(challenge.Id))!;
    }

    public async Task<WeeklyChallengeDto?> UpdateAsync(int id, CreateWeeklyChallengeDto dto)
    {
        var challenge = await _db.WeeklyChallenges.FirstOrDefaultAsync(c => c.Id == id);
        if (challenge == null) return null;
        if (challenge.IsTeamChallenge != dto.IsTeamChallenge
            && await _db.WeeklyChallenges.AnyAsync(c => c.Id == id && (c.UserChallenges.Any() || c.TeamChallengeEntries.Any())))
            throw new BusinessRuleException("The challenge type cannot change after participants have joined.");

        Apply(challenge, dto);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> SetActiveAsync(int id, bool isActive)
        => await _db.WeeklyChallenges.Where(c => c.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsActive, isActive)) > 0;

    public async Task<bool> JoinAsync(int challengeId, string userId)
    {
        var challenge = await LoadJoinableAsync(challengeId);
        if (challenge.IsTeamChallenge)
            throw new BusinessRuleException("This is a team challenge. Join it through your team.");
        if (await _db.UserChallenges.AnyAsync(x => x.UserId == userId && x.WeeklyChallengeId == challengeId))
            return false;
        if (challenge.MaxParticipants.HasValue
            && await _db.UserChallenges.CountAsync(x => x.WeeklyChallengeId == challengeId) >= challenge.MaxParticipants)
            throw new BusinessRuleException("This challenge is full.");

        _db.UserChallenges.Add(new UserChallenge { UserId = userId, WeeklyChallengeId = challengeId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> JoinAsTeamAsync(int challengeId, int teamId, string requestingUserId)
    {
        var challenge = await LoadJoinableAsync(challengeId);
        if (!challenge.IsTeamChallenge)
            throw new BusinessRuleException("This challenge is for individual participants.");

        var canEnroll = await _db.TeamMembers.AnyAsync(m => m.TeamId == teamId && m.UserId == requestingUserId
            && (m.Role == TeamRole.Leader || m.Role == TeamRole.CoLeader) && m.Team.IsActive);
        if (!canEnroll)
            throw new ForbiddenException("Only an active team's leader or co-leader can enroll the team.");

        if (await _db.TeamChallengeEntries.AnyAsync(x => x.TeamId == teamId && x.WeeklyChallengeId == challengeId))
            return false;
        if (challenge.MaxParticipants.HasValue
            && await _db.TeamChallengeEntries.CountAsync(x => x.WeeklyChallengeId == challengeId) >= challenge.MaxParticipants)
            throw new BusinessRuleException("This challenge is full.");

        _db.TeamChallengeEntries.Add(new TeamChallengeEntry { TeamId = teamId, WeeklyChallengeId = challengeId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ChallengeLeaderboardEntryDto>> GetLeaderboardAsync(int challengeId)
    {
        var rows = await _db.UserChallenges.AsNoTracking()
            .Where(uc => uc.WeeklyChallengeId == challengeId)
            .OrderByDescending(uc => uc.EarnedPoints).ThenBy(uc => uc.CompletedAt ?? DateTime.MaxValue)
            .Take(50)
            .Select(uc => new { uc.UserId, UserName = uc.User.UserName, uc.EarnedPoints, uc.CompletedExercises, uc.Status })
            .ToListAsync();

        return rows.Select((r, i) => new ChallengeLeaderboardEntryDto
        {
            Rank = i + 1,
            UserId = r.UserId,
            UserName = r.UserName ?? string.Empty,
            EarnedPoints = r.EarnedPoints,
            CompletedExercises = r.CompletedExercises,
            Status = r.Status.ToString()
        }).ToList();
    }

    private async Task<WeeklyChallenge> LoadJoinableAsync(int challengeId)
    {
        var challenge = await _db.WeeklyChallenges.AsNoTracking().FirstOrDefaultAsync(c => c.Id == challengeId)
            ?? throw new NotFoundException("Challenge not found.");
        if (!challenge.IsActive || challenge.EndDate < DateTime.UtcNow)
            throw new BusinessRuleException("This challenge is no longer open.");
        return challenge;
    }

    private static void Apply(WeeklyChallenge c, CreateWeeklyChallengeDto dto)
    {
        c.Title = dto.Title;
        c.Description = dto.Description;
        c.StartDate = dto.StartDate;
        c.EndDate = dto.EndDate;
        c.TargetExerciseCount = dto.TargetExerciseCount;
        c.TargetPoints = dto.TargetPoints;
        c.TargetAccuracy = dto.TargetAccuracy;
        c.BonusPoints = dto.BonusPoints;
        c.IsTeamChallenge = dto.IsTeamChallenge;
        c.ImageUrl = dto.ImageUrl;
    }
}
