using NeuroPath.Core.Models.Domain.BrainScience;

namespace NeuroPath.Core.Models.Domain.Content;

public class ResearchStudyBrainArea
{
    public int ResearchStudyId { get; set; }
    public int BrainAreaId { get; set; }
    public ResearchStudy ResearchStudy { get; set; } = null!;
    public BrainArea BrainArea { get; set; } = null!;
}
