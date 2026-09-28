namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExercisePrerequisite
{
    public int ExerciseId { get; set; }
    public int PrerequisiteExerciseId { get; set; }
    public bool IsRequired { get; set; }

    public Exercise Exercise { get; set; } = null!;
    public Exercise PrerequisiteExercise { get; set; } = null!;
}
