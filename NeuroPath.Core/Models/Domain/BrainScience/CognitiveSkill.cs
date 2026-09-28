using NeuroPath.Core.Models.Domain.Assessment;

namespace NeuroPath.Core.Models.Domain.BrainScience;

public class CognitiveSkill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MeasurementUnit { get; set; }

    public ICollection<CognitiveSkillBrainArea> CognitiveSkillBrainAreas { get; set; } = new List<CognitiveSkillBrainArea>();
    public ICollection<SkillAssessment> SkillAssessments { get; set; } = new List<SkillAssessment>();
}
