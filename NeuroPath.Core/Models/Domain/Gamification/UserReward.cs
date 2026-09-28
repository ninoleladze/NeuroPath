using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class UserReward
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int RewardId { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public string Source { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public Reward Reward { get; set; } = null!;
}
