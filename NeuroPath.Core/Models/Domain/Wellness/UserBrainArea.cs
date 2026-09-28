using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Wellness;

public class UserBrainArea
{
    public string UserId { get; set; } = string.Empty;
    public int BrainAreaId { get; set; }
    public int CurrentLevel { get; set; } = 1;
    public int ExperiencePoints { get; set; }
    public int TotalExercises { get; set; }
    public decimal AverageAccuracy { get; set; }
    public DateTime? LastTrainedAt { get; set; }
    public User User { get; set; } = null!;
    public BrainArea BrainArea { get; set; } = null!;
}
