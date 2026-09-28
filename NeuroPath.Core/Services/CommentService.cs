using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class CommentService : ICommentService
{
    private readonly NeuroPathDbContext _db;

    public CommentService(NeuroPathDbContext db) => _db = db;

    public async Task<List<CommentDto>> GetByArticleAsync(int articleId)
        => await _db.Comments.Where(c => c.ArticleId == articleId && c.ParentCommentId == null && !c.IsDeleted)
            .Include(c => c.User).Include(c => c.Replies).ThenInclude(r => r.User)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id, ArticleId = c.ArticleId, UserId = c.UserId, UserName = c.User.UserName!,
                Content = c.Content, ParentCommentId = c.ParentCommentId, LikeCount = c.LikeCount,
                CreatedAt = c.CreatedAt, IsEdited = c.IsEdited,
                Replies = c.Replies.Where(r => !r.IsDeleted).Select(r => new CommentDto
                {
                    Id = r.Id, ArticleId = r.ArticleId, UserId = r.UserId, UserName = r.User.UserName!,
                    Content = r.Content, ParentCommentId = r.ParentCommentId, LikeCount = r.LikeCount,
                    CreatedAt = r.CreatedAt, IsEdited = r.IsEdited
                }).ToList()
            }).ToListAsync();

    public async Task<CommentDto> CreateAsync(CreateCommentDto dto, string userId)
    {
        var c = new Comment { ArticleId = dto.ArticleId, UserId = userId, Content = dto.Content, ParentCommentId = dto.ParentCommentId };
        _db.Comments.Add(c);
        await _db.SaveChangesAsync();
        var user = await _db.Users.FindAsync(userId);
        return new CommentDto { Id = c.Id, ArticleId = c.ArticleId, UserId = c.UserId, UserName = user?.UserName ?? userId, Content = c.Content, ParentCommentId = c.ParentCommentId, CreatedAt = c.CreatedAt };
    }

    public async Task<CommentDto?> UpdateAsync(int id, string content, string userId)
    {
        var c = await _db.Comments.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (c == null) return null;
        c.Content = content; c.IsEdited = true; c.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new CommentDto { Id = c.Id, ArticleId = c.ArticleId, UserId = c.UserId, UserName = c.User.UserName!, Content = c.Content, LikeCount = c.LikeCount, CreatedAt = c.CreatedAt, IsEdited = c.IsEdited };
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var c = await _db.Comments.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (c == null) return false;
        c.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LikeAsync(int commentId, string userId)
    {
        if (await _db.CommentLikes.AnyAsync(x => x.CommentId == commentId && x.UserId == userId)) return false;
        _db.CommentLikes.Add(new CommentLike { CommentId = commentId, UserId = userId });
        var c = await _db.Comments.FindAsync(commentId);
        if (c != null) c.LikeCount++;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnlikeAsync(int commentId, string userId)
    {
        var like = await _db.CommentLikes.FirstOrDefaultAsync(x => x.CommentId == commentId && x.UserId == userId);
        if (like == null) return false;
        _db.CommentLikes.Remove(like);
        var c = await _db.Comments.FindAsync(commentId);
        if (c != null && c.LikeCount > 0) c.LikeCount--;
        await _db.SaveChangesAsync();
        return true;
    }
}
