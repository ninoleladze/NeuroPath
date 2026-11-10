using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Training;

public class UserExerciseLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ExerciseId { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public int Score { get; set; }
    public int DurationSeconds { get; set; }
    public decimal AccuracyPercent { get; set; }
    public int DifficultyAtCompletion { get; set; }
    public int PointsAwarded { get; set; }

    public User User { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}
