namespace NeuroPath.Core.Models.Domain.BrainScience;

public class Neurotransmitter
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ChemicalFormula { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Function { get; set; } = string.Empty;
    public string? BoostTips { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<BrainAreaNeurotransmitter> BrainAreaNeurotransmitters { get; set; } = new List<BrainAreaNeurotransmitter>();
}
