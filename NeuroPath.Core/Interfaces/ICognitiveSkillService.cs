using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.Core.Interfaces;

public interface ICognitiveSkillService
{
    Task<List<CognitiveSkillDto>> GetAllAsync();
    Task<CognitiveSkillDto?> GetByIdAsync(int id);
}
