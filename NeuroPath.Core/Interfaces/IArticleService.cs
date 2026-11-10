using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Interfaces;

public interface IArticleService
{
    Task<List<ArticleDto>> GetAllAsync(bool publishedOnly = true, int page = 1, int pageSize = 20);
    Task<ArticleDetailDto?> GetByIdAsync(int id, bool includeUnpublished = false);
    Task<List<ArticleDto>> GetByCategoryAsync(int categoryId, int page = 1, int pageSize = 20);
    Task<List<ArticleDto>> GetFeaturedAsync();
    Task<ArticleDetailDto> CreateAsync(CreateArticleDto dto, string userId);
    Task<ArticleDetailDto?> UpdateAsync(int id, CreateArticleDto dto, string userId, bool isAdmin);
    Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
    Task<bool> PublishAsync(int id, string userId, bool isAdmin);
    Task IncrementViewCountAsync(int id);
    Task<List<ArticleCategoryDto>> GetCategoriesAsync();
}
