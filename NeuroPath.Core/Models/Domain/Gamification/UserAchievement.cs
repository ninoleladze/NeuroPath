using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class UserAchievement
{
    public string UserId { get; set; } = string.Empty;
    public int AchievementId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public Achievement Achievement { get; set; } = null!;
}
