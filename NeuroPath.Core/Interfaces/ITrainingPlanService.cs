using NeuroPath.Core.Models.DTOs.Training;

namespace NeuroPath.Core.Interfaces;

public interface ITrainingPlanService
{
    Task<List<TrainingPlanDto>> GetUserPlansAsync(string userId);
    Task<TrainingPlanDto?> GetByIdAsync(int id, string userId);
    Task<TrainingPlanDto> CreateAsync(CreateTrainingPlanDto dto, string userId);
    Task<TrainingPlanDto?> UpdateAsync(int id, CreateTrainingPlanDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> AddExerciseAsync(int planId, int exerciseId, int dayOfWeek, int orderInDay, string userId);
    Task<bool> RemoveExerciseAsync(int planExerciseId, string userId);
}
