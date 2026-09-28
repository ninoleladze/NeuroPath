using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class LeaderboardEntry
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Period { get; set; } = "weekly";
    public int TotalPoints { get; set; }
    public int Rank { get; set; }
    public int ExercisesCompleted { get; set; }
    public int QuizzesCompleted { get; set; }
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
}
