using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Services;

public class TeamService : ITeamService
{
    private readonly INeuroPathDbContext _db;

    public TeamService(INeuroPathDbContext db) => _db = db;

    public async Task<List<TeamDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.Teams.AsNoTracking()
            .Where(t => t.IsActive)
            .OrderByDescending(t => t.TotalPoints).ThenBy(t => t.Name)
            .Skip(skip).Take(take)
            .Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                AvatarUrl = t.AvatarUrl,
                MaxMembers = t.MaxMembers,
                TotalPoints = t.TotalPoints,
                IsActive = t.IsActive,
                MemberCount = t.TeamMembers.Count,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<TeamDto?> GetByIdAsync(int id)
        => await _db.Teams.AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                AvatarUrl = t.AvatarUrl,
                MaxMembers = t.MaxMembers,
                TotalPoints = t.TotalPoints,
                IsActive = t.IsActive,
                MemberCount = t.TeamMembers.Count,
                CreatedAt = t.CreatedAt
            })
            .FirstOrDefaultAsync();

    public async Task<TeamDto> CreateAsync(CreateTeamDto dto, string userId)
    {
        var name = dto.Name.Trim();
        if (await _db.Teams.AnyAsync(t => t.Name == name))
            throw new ConflictException("A team with this name already exists.");

        var team = new Team { Name = name, Description = dto.Description, MaxMembers = dto.MaxMembers, CreatedByUserId = userId };
        team.TeamMembers.Add(new TeamMember { UserId = userId, Role = TeamRole.Leader });
        _db.Teams.Add(team);
        await _db.SaveChangesAsync();

        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            MaxMembers = team.MaxMembers,
            IsActive = team.IsActive,
            MemberCount = 1,
            CreatedAt = team.CreatedAt
        };
    }

    public async Task<TeamDto?> UpdateAsync(int teamId, CreateTeamDto dto, string requestingUserId)
    {
        var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == teamId && t.IsActive);
        if (team == null) return null;
        await EnsureRoleAsync(teamId, requestingUserId, TeamRole.Leader);

        var name = dto.Name.Trim();
        if (name != team.Name && await _db.Teams.AnyAsync(t => t.Name == name && t.Id != teamId))
            throw new ConflictException("A team with this name already exists.");
        var memberCount = await _db.TeamMembers.CountAsync(m => m.TeamId == teamId);
        if (dto.MaxMembers < memberCount)
            throw new BusinessRuleException($"The team already has {memberCount} members.");

        team.Name = name;
        team.Description = dto.Description;
        team.MaxMembers = dto.MaxMembers;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(teamId);
    }

    public async Task<bool> ArchiveAsync(int teamId, string requestingUserId, bool isAdmin)
    {
        var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == teamId && t.IsActive);
        if (team == null) return false;
        if (!isAdmin) await EnsureRoleAsync(teamId, requestingUserId, TeamRole.Leader);

        team.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveMemberAsync(int teamId, string targetUserId, string requestingUserId)
    {
        if (targetUserId == requestingUserId)
            throw new BusinessRuleException("Use leave to exit the team yourself.");

        var requester = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == requestingUserId);
        if (requester == null || requester.Role is not (TeamRole.Leader or TeamRole.CoLeader))
            throw new ForbiddenException("Only the team leader or a co-leader can remove members.");

        var target = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == targetUserId);
        if (target == null) return false;
        if (target.Role == TeamRole.Leader || (target.Role == TeamRole.CoLeader && requester.Role != TeamRole.Leader))
            throw new ForbiddenException("You cannot remove a member with the same or a higher role.");

        _db.TeamMembers.Remove(target);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> JoinAsync(int teamId, string userId)
    {
        var team = await _db.Teams.AsNoTracking()
            .Where(t => t.Id == teamId)
            .Select(t => new { t.IsActive, t.MaxMembers, Count = t.TeamMembers.Count, IsMember = t.TeamMembers.Any(m => m.UserId == userId) })
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Team not found.");

        if (!team.IsActive) throw new BusinessRuleException("This team is no longer active.");
        if (team.IsMember) return false;
        if (team.Count >= team.MaxMembers) throw new BusinessRuleException("This team is full.");

        _db.TeamMembers.Add(new TeamMember { UserId = userId, TeamId = teamId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LeaveAsync(int teamId, string userId)
    {
        var members = await _db.TeamMembers.Where(m => m.TeamId == teamId).ToListAsync();
        var leaving = members.FirstOrDefault(m => m.UserId == userId);
        if (leaving == null) return false;

        var remaining = members.Where(m => m.UserId != userId).ToList();
        if (leaving.Role == TeamRole.Leader && remaining.Count > 0)
        {
            var successor = remaining
                .OrderBy(m => m.Role == TeamRole.CoLeader ? 0 : 1)
                .ThenBy(m => m.JoinedAt)
                .First();
            successor.Role = TeamRole.Leader;
        }

        _db.TeamMembers.Remove(leaving);

        if (remaining.Count == 0)
        {
            var team = await _db.Teams.FirstAsync(t => t.Id == teamId);
            team.IsActive = false;
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetMemberRoleAsync(int teamId, string targetUserId, TeamRole role, string requestingUserId)
    {
        var requester = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == requestingUserId);
        if (requester == null || requester.Role != TeamRole.Leader)
            throw new ForbiddenException("Only the team leader can change member roles.");
        if (targetUserId == requestingUserId)
            throw new BusinessRuleException("Transfer leadership to another member instead of changing your own role.");

        var target = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == targetUserId);
        if (target == null) return false;

        target.Role = role;
        if (role == TeamRole.Leader)
            requester.Role = TeamRole.CoLeader;

        await _db.SaveChangesAsync();
        return true;
    }

    private async Task EnsureRoleAsync(int teamId, string userId, TeamRole role)
    {
        if (!await _db.TeamMembers.AnyAsync(m => m.TeamId == teamId && m.UserId == userId && m.Role == role))
            throw new ForbiddenException("Only the team leader can do this.");
    }

    public async Task<List<TeamMemberDto>> GetMembersAsync(int teamId)
        => await _db.TeamMembers.AsNoTracking()
            .Where(m => m.TeamId == teamId)
            .OrderBy(m => m.Role).ThenBy(m => m.JoinedAt)
            .Select(m => new TeamMemberDto
            {
                UserId = m.UserId,
                UserName = m.User.UserName!,
                AvatarUrl = m.User.AvatarUrl,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ContributedPoints = m.ContributedPoints
            })
            .ToListAsync();
}
