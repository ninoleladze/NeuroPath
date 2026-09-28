using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Challenges;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class Reward
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RewardType RewardType { get; set; }
    public int PointsValue { get; set; }
    public string? IconUrl { get; set; }
    public ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
    public ICollection<ChallengeReward> ChallengeRewards { get; set; } = new List<ChallengeReward>();
}
