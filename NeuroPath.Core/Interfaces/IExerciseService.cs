using NeuroPath.Core.Models.DTOs.Exercise;

namespace NeuroPath.Core.Interfaces;

public interface IExerciseService
{
    Task<List<ExerciseDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ExerciseDetailDto?> GetByIdAsync(int id, bool includeInactive = false);
    Task<ExerciseDto> CreateAsync(CreateExerciseDto dto, string userId);
    Task<ExerciseDto?> UpdateAsync(int id, UpdateExerciseDto dto, string userId, bool isAdmin);
    Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
    Task<List<ExerciseHintDto>> GetHintsAsync(int exerciseId);
    Task<ExerciseHintDto> AddHintAsync(int exerciseId, CreateExerciseHintDto dto, string userId, bool isAdmin);
    Task<ExerciseHintDto?> UpdateHintAsync(int exerciseId, int hintId, CreateExerciseHintDto dto, string userId, bool isAdmin);
    Task<bool> DeleteHintAsync(int exerciseId, int hintId, string userId, bool isAdmin);
    Task<List<ExerciseReviewDto>> GetReviewsAsync(int exerciseId);
    Task<ExerciseReviewDto> AddReviewAsync(int exerciseId, CreateExerciseReviewDto dto, string userId);
    Task<bool> ApproveReviewAsync(int reviewId);
    Task<List<ExerciseCategoryDto>> GetCategoriesAsync();
    Task<List<DifficultyLevelDto>> GetDifficultyLevelsAsync();
}
