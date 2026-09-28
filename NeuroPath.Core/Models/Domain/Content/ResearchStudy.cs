namespace NeuroPath.Core.Models.Domain.Content;

public class ResearchStudy
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public string? Journal { get; set; }
    public int PublishedYear { get; set; }
    public string? Doi { get; set; }
    public string? ExternalUrl { get; set; }
    public string KeyFindings { get; set; } = string.Empty;
    public string? PracticalImplications { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsVerified { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ResearchStudyBrainArea> ResearchStudyBrainAreas { get; set; } = new List<ResearchStudyBrainArea>();
}
