using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.Core.Interfaces;

public interface IBrainRegionService
{
    Task<List<BrainRegionDto>> GetAllAsync();
    Task<BrainRegionDto?> GetByIdAsync(int id);
    Task<BrainRegionDto> CreateAsync(CreateBrainRegionDto dto);
}
