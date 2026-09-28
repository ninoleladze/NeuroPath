using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Training;

public class DailySession
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? TrainingPlanId { get; set; }
    public DateTime SessionDate { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }
    public int TotalPoints { get; set; }
    public int TotalExercises { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.InProgress;
    public string? Notes { get; set; }

    public User User { get; set; } = null!;
    public TrainingPlan? TrainingPlan { get; set; }
    public ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();
}
