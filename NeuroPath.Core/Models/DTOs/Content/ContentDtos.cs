namespace NeuroPath.Core.Models.DTOs.Content;

public class ArticleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string AuthorUserName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int ReadTimeMinutes { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? PublishedAt { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class ArticleDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string AuthorUserId { get; set; } = string.Empty;
    public string AuthorUserName { get; set; } = string.Empty;
    public int ArticleCategoryId { get; set; }
    public int ReadTimeMinutes { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}

public class CreateArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int ArticleCategoryId { get; set; }
    public int ReadTimeMinutes { get; set; }
    public List<int> TagIds { get; set; } = new();
}

public class ArticleCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEdited { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
}

public class CreateCommentDto
{
    public int ArticleId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
}

public class BookmarkDto
{
    public int Id { get; set; }
    public string BookmarkType { get; set; } = string.Empty;
    public int ReferenceId { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBookmarkDto
{
    public string BookmarkType { get; set; } = string.Empty;
    public int ReferenceId { get; set; }
    public string? Note { get; set; }
}

public class ResearchStudyDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public string? Journal { get; set; }
    public int PublishedYear { get; set; }
    public string KeyFindings { get; set; } = string.Empty;
    public string? ExternalUrl { get; set; }
    public bool IsVerified { get; set; }
}

public class DailyTipDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
}

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
