using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.Core.Interfaces;

public interface INeurotransmitterService
{
    Task<List<NeurotransmitterDto>> GetAllAsync();
    Task<NeurotransmitterDto?> GetByIdAsync(int id);
}
