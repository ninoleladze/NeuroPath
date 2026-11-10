using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Services;

public class ArticleService : IArticleService
{
    private readonly INeuroPathDbContext _db;

    public ArticleService(INeuroPathDbContext db) => _db = db;

    public async Task<List<ArticleDto>> GetAllAsync(bool publishedOnly = true, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        var query = _db.Articles.AsNoTracking();
        if (publishedOnly) query = query.Where(a => a.IsPublished);
        return await query
            .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
            .Skip(skip).Take(take)
            .Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                ImageUrl = a.ImageUrl,
                AuthorUserName = a.Author.UserName!,
                CategoryName = a.ArticleCategory.Name,
                ReadTimeMinutes = a.ReadTimeMinutes,
                ViewCount = a.ViewCount,
                LikeCount = a.LikeCount,
                IsPublished = a.IsPublished,
                IsFeatured = a.IsFeatured,
                PublishedAt = a.PublishedAt,
                Tags = a.ArticleTags.Select(t => t.Tag.Name).ToList()
            })
            .ToListAsync();
    }

    public async Task<ArticleDetailDto?> GetByIdAsync(int id, bool includeUnpublished = false)
    {
        var article = await _db.Articles.AsNoTracking()
            .Where(a => a.Id == id && (includeUnpublished || a.IsPublished))
            .Select(a => new ArticleDetailDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Summary = a.Summary,
                ImageUrl = a.ImageUrl,
                AuthorUserId = a.AuthorUserId,
                AuthorUserName = a.Author.UserName!,
                ArticleCategoryId = a.ArticleCategoryId,
                ReadTimeMinutes = a.ReadTimeMinutes,
                ViewCount = a.ViewCount,
                LikeCount = a.LikeCount,
                IsPublished = a.IsPublished,
                PublishedAt = a.PublishedAt
            })
            .FirstOrDefaultAsync();
        if (article == null) return null;

        var comments = await _db.Comments.AsNoTracking()
            .Where(c => c.ArticleId == id && !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                ArticleId = c.ArticleId,
                UserId = c.UserId,
                UserName = c.User.UserName!,
                Content = c.Content,
                ParentCommentId = c.ParentCommentId,
                LikeCount = c.LikeCount,
                CreatedAt = c.CreatedAt,
                IsEdited = c.IsEdited
            })
            .ToListAsync();

        article.Comments = CommentTreeBuilder.Build(comments);
        return article;
    }

    public async Task<List<ArticleDto>> GetByCategoryAsync(int categoryId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.Articles.AsNoTracking()
            .Where(a => a.IsPublished && a.ArticleCategoryId == categoryId)
            .OrderByDescending(a => a.PublishedAt)
            .Skip(skip).Take(take)
            .Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                ImageUrl = a.ImageUrl,
                AuthorUserName = a.Author.UserName!,
                CategoryName = a.ArticleCategory.Name,
                ReadTimeMinutes = a.ReadTimeMinutes,
                ViewCount = a.ViewCount,
                LikeCount = a.LikeCount,
                IsPublished = a.IsPublished,
                IsFeatured = a.IsFeatured,
                PublishedAt = a.PublishedAt,
                Tags = a.ArticleTags.Select(t => t.Tag.Name).ToList()
            })
            .ToListAsync();
    }

    public async Task<List<ArticleDto>> GetFeaturedAsync()
        => await _db.Articles.AsNoTracking()
            .Where(a => a.IsPublished && a.IsFeatured)
            .OrderByDescending(a => a.PublishedAt)
            .Take(10)
            .Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                ImageUrl = a.ImageUrl,
                AuthorUserName = a.Author.UserName!,
                CategoryName = a.ArticleCategory.Name,
                ReadTimeMinutes = a.ReadTimeMinutes,
                ViewCount = a.ViewCount,
                LikeCount = a.LikeCount,
                IsPublished = a.IsPublished,
                IsFeatured = a.IsFeatured,
                PublishedAt = a.PublishedAt,
                Tags = a.ArticleTags.Select(t => t.Tag.Name).ToList()
            })
            .ToListAsync();

    public async Task<ArticleDetailDto> CreateAsync(CreateArticleDto dto, string userId)
    {
        await EnsureCategoryExistsAsync(dto.ArticleCategoryId);
        var tagIds = await ResolveTagIdsAsync(dto.TagIds);

        var article = new Article
        {
            Title = dto.Title,
            Content = dto.Content,
            Summary = dto.Summary,
            ImageUrl = dto.ImageUrl,
            AuthorUserId = userId,
            ArticleCategoryId = dto.ArticleCategoryId,
            ReadTimeMinutes = dto.ReadTimeMinutes
        };
        foreach (var tagId in tagIds)
            article.ArticleTags.Add(new ArticleTag { TagId = tagId });

        _db.Articles.Add(article);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(article.Id, includeUnpublished: true))!;
    }

    public async Task<ArticleDetailDto?> UpdateAsync(int id, CreateArticleDto dto, string userId, bool isAdmin)
    {
        var article = await _db.Articles.Include(a => a.ArticleTags).FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return null;
        EnsureCanModify(article, userId, isAdmin);
        await EnsureCategoryExistsAsync(dto.ArticleCategoryId);
        var tagIds = await ResolveTagIdsAsync(dto.TagIds);

        article.Title = dto.Title;
        article.Content = dto.Content;
        article.Summary = dto.Summary;
        article.ImageUrl = dto.ImageUrl;
        article.ArticleCategoryId = dto.ArticleCategoryId;
        article.ReadTimeMinutes = dto.ReadTimeMinutes;
        article.UpdatedAt = DateTime.UtcNow;

        article.ArticleTags.Clear();
        foreach (var tagId in tagIds)
            article.ArticleTags.Add(new ArticleTag { ArticleId = article.Id, TagId = tagId });

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id, includeUnpublished: true);
    }

    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var article = await _db.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;
        EnsureCanModify(article, userId, isAdmin);

        await using var transaction = await _db.Database.BeginTransactionAsync();
        await _db.CommentLikes.Where(l => l.Comment.ArticleId == id).ExecuteDeleteAsync();
        await _db.Comments.Where(c => c.ArticleId == id && c.ParentCommentId != null).ExecuteUpdateAsync(s => s.SetProperty(c => c.ParentCommentId, (int?)null));
        await _db.Comments.Where(c => c.ArticleId == id).ExecuteDeleteAsync();
        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }

    public async Task<bool> PublishAsync(int id, string userId, bool isAdmin)
    {
        var article = await _db.Articles.FirstOrDefaultAsync(a => a.Id == id);
        if (article == null) return false;
        EnsureCanModify(article, userId, isAdmin);
        if (article.IsPublished) return true;
        article.IsPublished = true;
        article.PublishedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task IncrementViewCountAsync(int id)
        => await _db.Articles
            .Where(a => a.Id == id && a.IsPublished)
            .ExecuteUpdateAsync(s => s.SetProperty(a => a.ViewCount, a => a.ViewCount + 1));

    public async Task<List<ArticleCategoryDto>> GetCategoriesAsync()
        => await _db.ArticleCategories.AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new ArticleCategoryDto { Id = c.Id, Name = c.Name, Description = c.Description, IconUrl = c.IconUrl })
            .ToListAsync();

    private static void EnsureCanModify(Article article, string userId, bool isAdmin)
    {
        if (!isAdmin && article.AuthorUserId != userId)
            throw new ForbiddenException("Only the author or an administrator can modify this article.");
    }

    private async Task EnsureCategoryExistsAsync(int categoryId)
    {
        if (!await _db.ArticleCategories.AnyAsync(c => c.Id == categoryId))
            throw new BusinessRuleException($"Article category {categoryId} does not exist.");
    }

    private async Task<List<int>> ResolveTagIdsAsync(IEnumerable<int> requested)
    {
        var distinct = requested.Distinct().ToList();
        if (distinct.Count == 0) return distinct;
        var existing = await _db.Tags.Where(t => distinct.Contains(t.Id)).Select(t => t.Id).ToListAsync();
        var missing = distinct.Except(existing).ToList();
        if (missing.Count > 0)
            throw new BusinessRuleException($"Unknown tag ids: {string.Join(", ", missing)}.");
        return existing;
    }
}
