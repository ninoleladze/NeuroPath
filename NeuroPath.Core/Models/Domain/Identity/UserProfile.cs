using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Core.Models.Domain.Identity;

public class UserProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Occupation { get; set; }
    public string? CognitiveGoal { get; set; }
    public int DailyTrainingMinutes { get; set; } = 15;
    public int? PreferredDifficultyId { get; set; }

    public User User { get; set; } = null!;
    public DifficultyLevel? PreferredDifficulty { get; set; }
}
