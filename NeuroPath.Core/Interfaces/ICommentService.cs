using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Interfaces;

public interface ICommentService
{
    Task<List<CommentDto>> GetByArticleAsync(int articleId, int page = 1, int pageSize = 20);
    Task<CommentDto> CreateAsync(CreateCommentDto dto, string userId);
    Task<CommentDto?> UpdateAsync(int id, string content, string userId);
    Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
    Task<bool> LikeAsync(int commentId, string userId);
    Task<bool> UnlikeAsync(int commentId, string userId);
}
