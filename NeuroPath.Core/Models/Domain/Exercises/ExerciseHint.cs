namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExerciseHint
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string HintText { get; set; } = string.Empty;
    public int HintOrder { get; set; }
    public int PointPenalty { get; set; }

    public Exercise Exercise { get; set; } = null!;
}
