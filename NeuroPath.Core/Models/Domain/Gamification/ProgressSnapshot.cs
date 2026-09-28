using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class ProgressSnapshot
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime SnapshotDate { get; set; } = DateTime.UtcNow;
    public int TotalPoints { get; set; }
    public int ExercisesCompleted { get; set; }
    public int QuizzesCompleted { get; set; }
    public int CourseLessonsCompleted { get; set; }
    public decimal AverageAccuracy { get; set; }
    public int TotalTrainingMinutes { get; set; }
    public int StreakDays { get; set; }
    public string? TopBrainArea { get; set; }
    public string? WeakestBrainArea { get; set; }
    public decimal? MoodAverage { get; set; }
    public User User { get; set; } = null!;
}
