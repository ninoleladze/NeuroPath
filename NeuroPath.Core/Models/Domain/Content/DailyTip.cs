namespace NeuroPath.Core.Models.Domain.Content;

public class DailyTip
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? DisplayDate { get; set; }
}
