using NeuroPath.Core.Models.DTOs.Assessment;

namespace NeuroPath.Core.Interfaces;

public interface ISkillAssessmentService
{
    Task<List<SkillAssessmentDto>> GetAllAsync();
    Task<SkillAssessmentDetailDto?> GetByIdAsync(int id);
    Task<SkillAssessmentDto> CreateAsync(CreateSkillAssessmentDto dto);
    Task<AssessmentQuestionDto> AddQuestionAsync(int assessmentId, CreateAssessmentQuestionDto dto);
    Task<UserAssessmentResultDto> SubmitAsync(int assessmentId, SubmitAssessmentDto dto, string userId);
    Task<List<UserAssessmentResultDto>> GetUserResultsAsync(string userId);
}
