using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Interfaces;

public interface ITeamService
{
    Task<List<TeamDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<TeamDto?> GetByIdAsync(int id);
    Task<TeamDto> CreateAsync(CreateTeamDto dto, string userId);
    Task<TeamDto?> UpdateAsync(int teamId, CreateTeamDto dto, string requestingUserId);
    Task<bool> ArchiveAsync(int teamId, string requestingUserId, bool isAdmin);
    Task<bool> RemoveMemberAsync(int teamId, string targetUserId, string requestingUserId);
    Task<bool> JoinAsync(int teamId, string userId);
    Task<bool> LeaveAsync(int teamId, string userId);
    Task<bool> SetMemberRoleAsync(int teamId, string targetUserId, TeamRole role, string requestingUserId);
    Task<List<TeamMemberDto>> GetMembersAsync(int teamId);
}
