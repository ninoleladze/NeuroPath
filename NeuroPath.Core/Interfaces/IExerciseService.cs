using NeuroPath.Core.Models.DTOs.Exercise;

namespace NeuroPath.Core.Interfaces;

public interface IExerciseService
{
    Task<List<ExerciseDto>> GetAllAsync();
    Task<ExerciseDetailDto?> GetByIdAsync(int id);
    Task<ExerciseDto> CreateAsync(CreateExerciseDto dto, string userId);
    Task<ExerciseDto?> UpdateAsync(int id, UpdateExerciseDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<ExerciseHintDto>> GetHintsAsync(int exerciseId);
    Task<ExerciseHintDto> AddHintAsync(int exerciseId, CreateExerciseHintDto dto);
    Task<List<ExerciseReviewDto>> GetReviewsAsync(int exerciseId);
    Task<ExerciseReviewDto> AddReviewAsync(int exerciseId, CreateExerciseReviewDto dto, string userId);
    Task<List<ExerciseCategoryDto>> GetCategoriesAsync();
    Task<List<DifficultyLevelDto>> GetDifficultyLevelsAsync();
}
