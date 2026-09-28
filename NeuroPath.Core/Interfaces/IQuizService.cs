using NeuroPath.Core.Models.DTOs.Quiz;

namespace NeuroPath.Core.Interfaces;

public interface IQuizService
{
    Task<List<QuizDto>> GetAllAsync();
    Task<QuizDetailDto?> GetByIdAsync(int id);
    Task<QuizDto> CreateAsync(CreateQuizDto dto, string userId);
    Task<bool> DeleteAsync(int id);
    Task<QuizAttemptResultDto> SubmitAsync(SubmitQuizDto dto, string userId);
    Task<List<QuizAttemptResultDto>> GetUserHistoryAsync(string userId);
}
