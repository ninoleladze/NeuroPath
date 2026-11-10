using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Services;

public class CommentService : ICommentService
{
    private readonly INeuroPathDbContext _db;

    public CommentService(INeuroPathDbContext db) => _db = db;

    public async Task<List<CommentDto>> GetByArticleAsync(int articleId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);

        var rootIds = await _db.Comments.AsNoTracking()
            .Where(c => c.ArticleId == articleId && c.ParentCommentId == null && !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .Skip(skip).Take(take)
            .Select(c => c.Id)
            .ToListAsync();
        if (rootIds.Count == 0) return new List<CommentDto>();

        var all = await _db.Comments.AsNoTracking()
            .Where(c => c.ArticleId == articleId && !c.IsDeleted)
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

        var rootSet = rootIds.ToHashSet();
        return CommentTreeBuilder.Build(all).Where(c => rootSet.Contains(c.Id)).ToList();
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto dto, string userId)
    {
        if (!await _db.Articles.AnyAsync(a => a.Id == dto.ArticleId && a.IsPublished))
            throw new NotFoundException("Article not found.");

        if (dto.ParentCommentId.HasValue
            && !await _db.Comments.AnyAsync(c => c.Id == dto.ParentCommentId && c.ArticleId == dto.ArticleId && !c.IsDeleted))
            throw new BusinessRuleException("The parent comment does not exist on this article.");

        var comment = new Comment { ArticleId = dto.ArticleId, UserId = userId, Content = dto.Content.Trim(), ParentCommentId = dto.ParentCommentId };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();

        var userName = await _db.Users.Where(u => u.Id == userId).Select(u => u.UserName).FirstOrDefaultAsync();
        return new CommentDto
        {
            Id = comment.Id,
            ArticleId = comment.ArticleId,
            UserId = userId,
            UserName = userName ?? string.Empty,
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            CreatedAt = comment.CreatedAt
        };
    }

    public async Task<CommentDto?> UpdateAsync(int id, string content, string userId)
    {
        var comment = await _db.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId && !c.IsDeleted);
        if (comment == null) return null;

        comment.Content = content.Trim();
        comment.IsEdited = true;
        comment.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new CommentDto
        {
            Id = comment.Id,
            ArticleId = comment.ArticleId,
            UserId = comment.UserId,
            UserName = comment.User.UserName!,
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            LikeCount = comment.LikeCount,
            CreatedAt = comment.CreatedAt,
            IsEdited = comment.IsEdited
        };
    }

    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
        => await _db.Comments
            .Where(c => c.Id == id && !c.IsDeleted && (isAdmin || c.UserId == userId))
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsDeleted, true).SetProperty(c => c.UpdatedAt, DateTime.UtcNow)) > 0;

    public async Task<bool> LikeAsync(int commentId, string userId)
    {
        if (!await _db.Comments.AnyAsync(c => c.Id == commentId && !c.IsDeleted))
            throw new NotFoundException("Comment not found.");
        if (await _db.CommentLikes.AnyAsync(l => l.CommentId == commentId && l.UserId == userId))
            return false;

        await using var transaction = await _db.Database.BeginTransactionAsync();
        _db.CommentLikes.Add(new CommentLike { CommentId = commentId, UserId = userId });
        await _db.SaveChangesAsync();
        await _db.Comments.Where(c => c.Id == commentId)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.LikeCount, c => c.LikeCount + 1));
        await transaction.CommitAsync();
        return true;
    }

    public async Task<bool> UnlikeAsync(int commentId, string userId)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();
        var removed = await _db.CommentLikes.Where(l => l.CommentId == commentId && l.UserId == userId).ExecuteDeleteAsync();
        if (removed == 0) return false;
        await _db.Comments.Where(c => c.Id == commentId && c.LikeCount > 0)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.LikeCount, c => c.LikeCount - 1));
        await transaction.CommitAsync();
        return true;
    }
}
