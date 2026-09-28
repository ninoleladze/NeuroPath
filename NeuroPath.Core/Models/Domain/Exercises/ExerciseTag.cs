using NeuroPath.Core.Models.Domain.Content;

namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExerciseTag
{
    public int ExerciseId { get; set; }
    public int TagId { get; set; }

    public Exercise Exercise { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
