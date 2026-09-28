using NeuroPath.Core.Models.DTOs.Assessment;

namespace NeuroPath.Core.Interfaces;

public interface ISkillAssessmentService
{
    Task<List<SkillAssessmentDto>> GetAllAsync();
    Task<SkillAssessmentDto?> GetByIdAsync(int id);
    Task<SkillAssessmentDto> CreateAsync(CreateSkillAssessmentDto dto);
    Task<UserAssessmentResultDto> SubmitAsync(int assessmentId, string userId, int score, int maxScore, int timeTakenSeconds);
    Task<List<UserAssessmentResultDto>> GetUserResultsAsync(string userId);
}
