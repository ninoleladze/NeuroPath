using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Core.Models.Domain.Training;

public class SessionExercise
{
    public int Id { get; set; }
    public int DailySessionId { get; set; }
    public int ExerciseId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public decimal AccuracyPercent { get; set; }
    public int HintsUsed { get; set; }
    public int OrderInSession { get; set; }

    public DailySession DailySession { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}
