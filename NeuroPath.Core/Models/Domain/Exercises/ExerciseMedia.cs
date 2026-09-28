using NeuroPath.Core.Enums;

namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExerciseMedia
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
    public string? Title { get; set; }
    public int DisplayOrder { get; set; }
    public long? FileSize { get; set; }

    public Exercise Exercise { get; set; } = null!;
}
