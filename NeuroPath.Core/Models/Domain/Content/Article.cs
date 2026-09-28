using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Content;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string AuthorUserId { get; set; } = string.Empty;
    public int ArticleCategoryId { get; set; }
    public int ReadTimeMinutes { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsPublished { get; set; } = false;
    public bool IsFeatured { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User Author { get; set; } = null!;
    public ArticleCategory ArticleCategory { get; set; } = null!;
    public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
