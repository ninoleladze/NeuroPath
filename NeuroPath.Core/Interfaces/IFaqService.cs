using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IFaqService
{
    Task<List<FaqCategoryDto>> GetAllWithFaqsAsync();
    Task<List<FaqCategoryDto>> GetCategoriesAsync();
    Task<FaqDto> CreateFaqAsync(CreateFaqDto dto);
    Task<FaqDto?> UpdateFaqAsync(int id, CreateFaqDto dto);
    Task<bool> DeleteFaqAsync(int id);
}
