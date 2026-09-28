using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Social;

public class TeamMember
{
    public string UserId { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public TeamRole Role { get; set; } = TeamRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public int ContributedPoints { get; set; }
    public User User { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
