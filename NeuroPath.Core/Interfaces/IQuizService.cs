using NeuroPath.Core.Models.DTOs.Quiz;

namespace NeuroPath.Core.Interfaces;

public interface IQuizService
{
    Task<List<QuizDto>> GetAllAsync();
    Task<QuizDetailDto?> GetByIdAsync(int id, bool includeUnpublished = false);
    Task<QuizDto> CreateAsync(CreateQuizDto dto, string userId);
    Task<QuizQuestionDto> AddQuestionAsync(int quizId, CreateQuizQuestionDto dto, string userId, bool isAdmin);
    Task<bool> PublishAsync(int id, string userId, bool isAdmin);
    Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
    Task<QuizStartDto> StartAsync(int quizId, string userId);
    Task<QuizAttemptResultDto> SubmitAsync(SubmitQuizDto dto, string userId);
    Task<List<QuizAttemptResultDto>> GetUserHistoryAsync(string userId, int page = 1, int pageSize = 20);
}
