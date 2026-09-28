using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class ArticleService : IArticleService
{
    private readonly NeuroPathDbContext _db;

    public ArticleService(NeuroPathDbContext db) => _db = db;

    public async Task<List<ArticleDto>> GetAllAsync(bool publishedOnly = true)
    {
        var query = _db.Articles.Include(a => a.Author).Include(a => a.ArticleCategory)
            .Include(a => a.ArticleTags).ThenInclude(at => at.Tag).AsQueryable();
        if (publishedOnly) query = query.Where(a => a.IsPublished);
        return await query.OrderByDescending(a => a.PublishedAt)
            .Select(a => new ArticleDto
            {
                Id = a.Id, Title = a.Title, Summary = a.Summary, ImageUrl = a.ImageUrl,
                AuthorUserName = a.Author.UserName!, CategoryName = a.ArticleCategory.Name,
                ReadTimeMinutes = a.ReadTimeMinutes, ViewCount = a.ViewCount, LikeCount = a.LikeCount,
                IsPublished = a.IsPublished, IsFeatured = a.IsFeatured, PublishedAt = a.PublishedAt,
                Tags = a.ArticleTags.Select(t => t.Tag.Name).ToList()
            }).ToListAsync();
    }

    public async Task<ArticleDetailDto?> GetByIdAsync(int id)
    {
        var a = await _db.Articles.Include(x => x.Author).Include(x => x.ArticleCategory)
            .Include(x => x.Comments).ThenInclude(c => c.User)
            .Include(x => x.Comments).ThenInclude(c => c.Replies).ThenInclude(r => r.User)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) return null;
        return new ArticleDetailDto
        {
            Id = a.Id, Title = a.Title, Content = a.Content, Summary = a.Summary,
            ImageUrl = a.ImageUrl, AuthorUserId = a.AuthorUserId, AuthorUserName = a.Author.UserName!,
            ArticleCategoryId = a.ArticleCategoryId, ReadTimeMinutes = a.ReadTimeMinutes,
            ViewCount = a.ViewCount, LikeCount = a.LikeCount, IsPublished = a.IsPublished,
            PublishedAt = a.PublishedAt,
            Comments = a.Comments.Where(c => c.ParentCommentId == null && !c.IsDeleted)
                .Select(c => MapComment(c)).ToList()
        };
    }

    private static CommentDto MapComment(Comment c) => new()
    {
        Id = c.Id, ArticleId = c.ArticleId, UserId = c.UserId, UserName = c.User.UserName!,
        Content = c.Content, ParentCommentId = c.ParentCommentId, LikeCount = c.LikeCount,
        CreatedAt = c.CreatedAt, IsEdited = c.IsEdited,
        Replies = c.Replies.Where(r => !r.IsDeleted).Select(r => MapComment(r)).ToList()
    };

    public async Task<List<ArticleDto>> GetByCategoryAsync(int categoryId)
        => await _db.Articles.Where(a => a.IsPublished && a.ArticleCategoryId == categoryId)
            .Include(a => a.Author).Include(a => a.ArticleCategory)
            .Select(a => new ArticleDto
            {
                Id = a.Id, Title = a.Title, Summary = a.Summary, ImageUrl = a.ImageUrl,
                AuthorUserName = a.Author.UserName!, CategoryName = a.ArticleCategory.Name,
                ReadTimeMinutes = a.ReadTimeMinutes, ViewCount = a.ViewCount
            }).ToListAsync();

    public async Task<List<ArticleDto>> GetFeaturedAsync()
        => await _db.Articles.Where(a => a.IsPublished && a.IsFeatured)
            .Include(a => a.Author).Include(a => a.ArticleCategory)
            .OrderByDescending(a => a.PublishedAt)
            .Select(a => new ArticleDto
            {
                Id = a.Id, Title = a.Title, Summary = a.Summary, ImageUrl = a.ImageUrl,
                AuthorUserName = a.Author.UserName!, CategoryName = a.ArticleCategory.Name,
                ReadTimeMinutes = a.ReadTimeMinutes, ViewCount = a.ViewCount, IsFeatured = a.IsFeatured
            }).ToListAsync();

    public async Task<ArticleDetailDto> CreateAsync(CreateArticleDto dto, string userId)
    {
        var a = new Article
        {
            Title = dto.Title, Content = dto.Content, Summary = dto.Summary, ImageUrl = dto.ImageUrl,
            AuthorUserId = userId, ArticleCategoryId = dto.ArticleCategoryId,
            ReadTimeMinutes = dto.ReadTimeMinutes
        };
        _db.Articles.Add(a);
        await _db.SaveChangesAsync();
        foreach (var tagId in dto.TagIds)
            _db.ArticleTags.Add(new ArticleTag { ArticleId = a.Id, TagId = tagId });
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(a.Id))!;
    }

    public async Task<ArticleDetailDto?> UpdateAsync(int id, CreateArticleDto dto)
    {
        var a = await _db.Articles.FindAsync(id);
        if (a == null) return null;
        a.Title = dto.Title; a.Content = dto.Content; a.Summary = dto.Summary;
        a.ImageUrl = dto.ImageUrl; a.ArticleCategoryId = dto.ArticleCategoryId;
        a.ReadTimeMinutes = dto.ReadTimeMinutes; a.UpdatedAt = DateTime.UtcNow;
        var existing = _db.ArticleTags.Where(at => at.ArticleId == id);
        _db.ArticleTags.RemoveRange(existing);
        foreach (var tagId in dto.TagIds)
            _db.ArticleTags.Add(new ArticleTag { ArticleId = a.Id, TagId = tagId });
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var a = await _db.Articles.FindAsync(id);
        if (a == null) return false;
        _db.Articles.Remove(a);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PublishAsync(int id)
    {
        var a = await _db.Articles.FindAsync(id);
        if (a == null) return false;
        a.IsPublished = true;
        a.PublishedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task IncrementViewCountAsync(int id)
    {
        var a = await _db.Articles.FindAsync(id);
        if (a != null) { a.ViewCount++; await _db.SaveChangesAsync(); }
    }

    public async Task<List<ArticleCategoryDto>> GetCategoriesAsync()
        => await _db.ArticleCategories
            .Select(c => new ArticleCategoryDto { Id = c.Id, Name = c.Name, Description = c.Description, IconUrl = c.IconUrl })
            .ToListAsync();
}
