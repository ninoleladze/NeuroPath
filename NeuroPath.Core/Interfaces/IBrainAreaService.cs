using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.Core.Interfaces;

public interface IBrainAreaService
{
    Task<List<BrainAreaDto>> GetAllAsync();
    Task<BrainAreaDetailDto?> GetByIdAsync(int id);
    Task<BrainAreaDto> CreateAsync(CreateBrainAreaDto dto);
    Task<BrainAreaDto?> UpdateAsync(int id, CreateBrainAreaDto dto);
    Task<bool> DeleteAsync(int id);
}
