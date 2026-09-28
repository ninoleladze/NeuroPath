namespace NeuroPath.Core.Models.Domain.Admin;

public class Faq
{
    public int Id { get; set; }
    public int FaqCategoryId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsPublished { get; set; } = true;
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public FaqCategory FaqCategory { get; set; } = null!;
}
