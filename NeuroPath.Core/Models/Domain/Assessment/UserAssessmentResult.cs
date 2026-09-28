using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Assessment;

public class UserAssessmentResult
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int SkillAssessmentId { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public decimal ScorePercent { get; set; }
    public bool IsPassed { get; set; }
    public int TimeTakenSeconds { get; set; }

    public User User { get; set; } = null!;
    public SkillAssessment SkillAssessment { get; set; } = null!;
}
