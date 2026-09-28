using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.DTOs.Social;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class TeamService : ITeamService
{
    private readonly NeuroPathDbContext _db;

    public TeamService(NeuroPathDbContext db) => _db = db;

    public async Task<List<TeamDto>> GetAllAsync()
        => await _db.Teams.Where(t => t.IsActive)
            .Include(t => t.TeamMembers)
            .Select(t => new TeamDto
            {
                Id = t.Id, Name = t.Name, Description = t.Description, AvatarUrl = t.AvatarUrl,
                MaxMembers = t.MaxMembers, TotalPoints = t.TotalPoints, IsActive = t.IsActive,
                MemberCount = t.TeamMembers.Count, CreatedAt = t.CreatedAt
            }).ToListAsync();

    public async Task<TeamDto?> GetByIdAsync(int id)
    {
        var t = await _db.Teams.Include(x => x.TeamMembers).FirstOrDefaultAsync(x => x.Id == id);
        if (t == null) return null;
        return new TeamDto { Id = t.Id, Name = t.Name, Description = t.Description, AvatarUrl = t.AvatarUrl, MaxMembers = t.MaxMembers, TotalPoints = t.TotalPoints, IsActive = t.IsActive, MemberCount = t.TeamMembers.Count, CreatedAt = t.CreatedAt };
    }

    public async Task<TeamDto> CreateAsync(CreateTeamDto dto, string userId)
    {
        var t = new Team { Name = dto.Name, Description = dto.Description, MaxMembers = dto.MaxMembers, CreatedByUserId = userId };
        _db.Teams.Add(t);
        await _db.SaveChangesAsync();
        _db.TeamMembers.Add(new TeamMember { UserId = userId, TeamId = t.Id, Role = TeamRole.Leader });
        await _db.SaveChangesAsync();
        return new TeamDto { Id = t.Id, Name = t.Name, Description = t.Description, MaxMembers = t.MaxMembers, MemberCount = 1, CreatedAt = t.CreatedAt };
    }

    public async Task<bool> JoinAsync(int teamId, string userId)
    {
        var t = await _db.Teams.Include(x => x.TeamMembers).FirstOrDefaultAsync(x => x.Id == teamId);
        if (t == null || !t.IsActive || t.TeamMembers.Count >= t.MaxMembers) return false;
        if (t.TeamMembers.Any(m => m.UserId == userId)) return false;
        _db.TeamMembers.Add(new TeamMember { UserId = userId, TeamId = teamId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LeaveAsync(int teamId, string userId)
    {
        var member = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
        if (member == null) return false;
        _db.TeamMembers.Remove(member);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetMemberRoleAsync(int teamId, string targetUserId, string role, string requestingUserId)
    {
        var requester = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == requestingUserId);
        if (requester == null || requester.Role != TeamRole.Leader) return false;
        var member = await _db.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == targetUserId);
        if (member == null) return false;
        if (Enum.TryParse<TeamRole>(role, out var teamRole)) { member.Role = teamRole; await _db.SaveChangesAsync(); return true; }
        return false;
    }

    public async Task<List<TeamMemberDto>> GetMembersAsync(int teamId)
        => await _db.TeamMembers.Where(m => m.TeamId == teamId)
            .Include(m => m.User)
            .Select(m => new TeamMemberDto
            {
                UserId = m.UserId, UserName = m.User.UserName!, AvatarUrl = m.User.AvatarUrl,
                Role = m.Role, JoinedAt = m.JoinedAt, ContributedPoints = m.ContributedPoints
            }).ToListAsync();
}
