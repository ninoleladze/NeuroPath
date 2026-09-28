namespace NeuroPath.Core.Models.Domain.BrainScience;

public class CognitiveSkillBrainArea
{
    public int CognitiveSkillId { get; set; }
    public int BrainAreaId { get; set; }
    public int Contribution { get; set; }

    public CognitiveSkill CognitiveSkill { get; set; } = null!;
    public BrainArea BrainArea { get; set; } = null!;
}
