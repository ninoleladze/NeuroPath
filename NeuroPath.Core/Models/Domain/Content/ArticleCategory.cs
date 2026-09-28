namespace NeuroPath.Core.Models.Domain.Content;

public class ArticleCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
