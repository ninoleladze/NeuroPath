using NeuroPath.Core.Models.Domain.Gamification;

namespace NeuroPath.Core.Models.Domain.Challenges;

public class ChallengeReward
{
    public int Id { get; set; }
    public int WeeklyChallengeId { get; set; }
    public int RewardId { get; set; }
    public int PlaceRequired { get; set; }
    public WeeklyChallenge WeeklyChallenge { get; set; } = null!;
    public Reward Reward { get; set; } = null!;
}
