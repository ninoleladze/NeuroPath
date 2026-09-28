using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Exercises;

public class ExerciseReview
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsApproved { get; set; } = false;

    public Exercise Exercise { get; set; } = null!;
    public User User { get; set; } = null!;
}
