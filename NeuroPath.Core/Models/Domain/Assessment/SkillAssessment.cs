using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.BrainScience;

namespace NeuroPath.Core.Models.Domain.Assessment;

public class SkillAssessment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CognitiveSkillId { get; set; }
    public AssessmentType AssessmentType { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int TotalQuestions { get; set; }
    public int PassingScorePercent { get; set; } = 60;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public CognitiveSkill CognitiveSkill { get; set; } = null!;
    public ICollection<AssessmentQuestion> Questions { get; set; } = new List<AssessmentQuestion>();
    public ICollection<UserAssessmentResult> Results { get; set; } = new List<UserAssessmentResult>();
}
