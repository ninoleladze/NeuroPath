namespace NeuroPath.Core.Models.Domain.BrainScience;

public class BrainRegion
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LatinName { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Functions { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public ICollection<BrainRegionBrainArea> BrainRegionBrainAreas { get; set; } = new List<BrainRegionBrainArea>();
}
