using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class Streak
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime LastActivityDate { get; set; }
    public int TotalActiveDays { get; set; }
    public int StreakFreezeCount { get; set; }
    public User User { get; set; } = null!;
}
