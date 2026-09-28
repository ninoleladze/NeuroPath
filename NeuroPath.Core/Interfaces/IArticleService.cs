using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Interfaces;

public interface IArticleService
{
    Task<List<ArticleDto>> GetAllAsync(bool publishedOnly = true);
    Task<ArticleDetailDto?> GetByIdAsync(int id);
    Task<List<ArticleDto>> GetByCategoryAsync(int categoryId);
    Task<List<ArticleDto>> GetFeaturedAsync();
    Task<ArticleDetailDto> CreateAsync(CreateArticleDto dto, string userId);
    Task<ArticleDetailDto?> UpdateAsync(int id, CreateArticleDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> PublishAsync(int id);
    Task IncrementViewCountAsync(int id);
    Task<List<ArticleCategoryDto>> GetCategoriesAsync();
}
