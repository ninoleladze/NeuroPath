using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Wellness;

namespace NeuroPath.Core.Models.Domain.BrainScience;

public class BrainArea
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ScientificDescription { get; set; }
    public string? IconUrl { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; }

    public ICollection<BrainAreaNeurotransmitter> BrainAreaNeurotransmitters { get; set; } = new List<BrainAreaNeurotransmitter>();
    public ICollection<BrainRegionBrainArea> BrainRegionBrainAreas { get; set; } = new List<BrainRegionBrainArea>();
    public ICollection<CognitiveSkillBrainArea> CognitiveSkillBrainAreas { get; set; } = new List<CognitiveSkillBrainArea>();
    public ICollection<ExerciseBrainArea> ExerciseBrainAreas { get; set; } = new List<ExerciseBrainArea>();
    public ICollection<UserBrainArea> UserBrainAreas { get; set; } = new List<UserBrainArea>();
    public ICollection<ResearchStudyBrainArea> ResearchStudyBrainAreas { get; set; } = new List<ResearchStudyBrainArea>();
}
