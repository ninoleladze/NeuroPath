using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Core.Models.Domain.Training;

public class TrainingPlanExercise
{
    public int Id { get; set; }
    public int TrainingPlanId { get; set; }
    public int ExerciseId { get; set; }
    public int DayOfWeek { get; set; }
    public int OrderInDay { get; set; }
    public bool IsCompleted { get; set; }

    public TrainingPlan TrainingPlan { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}
