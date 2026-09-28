using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Social;

namespace NeuroPath.Core.Models.Domain.Challenges;

public class TeamChallengeEntry
{
    public int TeamId { get; set; }
    public int WeeklyChallengeId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public int TotalPoints { get; set; }
    public ChallengeStatus Status { get; set; } = ChallengeStatus.Joined;
    public DateTime? CompletedAt { get; set; }
    public Team Team { get; set; } = null!;
    public WeeklyChallenge WeeklyChallenge { get; set; } = null!;
}
