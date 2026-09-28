namespace NeuroPath.Core.Models.Domain.BrainScience;

public class BrainAreaNeurotransmitter
{
    public int BrainAreaId { get; set; }
    public int NeurotransmitterId { get; set; }
    public string? Role { get; set; }

    public BrainArea BrainArea { get; set; } = null!;
    public Neurotransmitter Neurotransmitter { get; set; } = null!;
}
