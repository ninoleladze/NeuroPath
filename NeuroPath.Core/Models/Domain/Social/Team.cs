using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.Domain.Challenges;

namespace NeuroPath.Core.Models.Domain.Social;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public int MaxMembers { get; set; } = 10;
    public int TotalPoints { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User CreatedByUser { get; set; } = null!;
    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    public ICollection<TeamChallengeEntry> TeamChallengeEntries { get; set; } = new List<TeamChallengeEntry>();
}
