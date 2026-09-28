using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.Core.Interfaces;

public interface ITeamService
{
    Task<List<TeamDto>> GetAllAsync();
    Task<TeamDto?> GetByIdAsync(int id);
    Task<TeamDto> CreateAsync(CreateTeamDto dto, string userId);
    Task<bool> JoinAsync(int teamId, string userId);
    Task<bool> LeaveAsync(int teamId, string userId);
    Task<bool> SetMemberRoleAsync(int teamId, string targetUserId, string role, string requestingUserId);
    Task<List<TeamMemberDto>> GetMembersAsync(int teamId);
}
