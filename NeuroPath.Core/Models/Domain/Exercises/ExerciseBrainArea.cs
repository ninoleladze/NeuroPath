using NeuroPath.Core.Models.Domain.BrainScience;

namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExerciseBrainArea
{
    public int ExerciseId { get; set; }
    public int BrainAreaId { get; set; }
    public int ImpactLevel { get; set; }
    public bool IsPrimary { get; set; }

    public Exercise Exercise { get; set; } = null!;
    public BrainArea BrainArea { get; set; } = null!;
}
