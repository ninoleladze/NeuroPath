namespace NeuroPath.Core.Models.Domain.BrainScience;

public class BrainRegionBrainArea
{
    public int BrainRegionId { get; set; }
    public int BrainAreaId { get; set; }
    public int ConnectionStrength { get; set; }

    public BrainRegion BrainRegion { get; set; } = null!;
    public BrainArea BrainArea { get; set; } = null!;
}
