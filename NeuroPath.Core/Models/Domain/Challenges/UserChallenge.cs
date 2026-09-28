using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Challenges;

public class UserChallenge
{
    public string UserId { get; set; } = string.Empty;
    public int WeeklyChallengeId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public int CompletedExercises { get; set; }
    public int EarnedPoints { get; set; }
    public decimal CurrentAccuracy { get; set; }
    public ChallengeStatus Status { get; set; } = ChallengeStatus.Joined;
    public DateTime? CompletedAt { get; set; }
    public User User { get; set; } = null!;
    public WeeklyChallenge WeeklyChallenge { get; set; } = null!;
}
