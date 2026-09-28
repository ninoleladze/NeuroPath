using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Core.Models.DTOs.Training;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class TrainingPlanService : ITrainingPlanService
{
    private readonly NeuroPathDbContext _db;

    public TrainingPlanService(NeuroPathDbContext db) => _db = db;

    public async Task<List<TrainingPlanDto>> GetUserPlansAsync(string userId)
        => await _db.TrainingPlans.Where(p => p.UserId == userId)
            .Include(p => p.Exercises)
            .Select(p => new TrainingPlanDto { Id = p.Id, Title = p.Title, Description = p.Description, StartDate = p.StartDate, EndDate = p.EndDate, IsActive = p.IsActive, CreatedAt = p.CreatedAt, ExerciseCount = p.Exercises.Count })
            .ToListAsync();

    public async Task<TrainingPlanDto?> GetByIdAsync(int id, string userId)
    {
        var p = await _db.TrainingPlans.Include(x => x.Exercises).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (p == null) return null;
        return new TrainingPlanDto { Id = p.Id, Title = p.Title, Description = p.Description, StartDate = p.StartDate, EndDate = p.EndDate, IsActive = p.IsActive, CreatedAt = p.CreatedAt, ExerciseCount = p.Exercises.Count };
    }

    public async Task<TrainingPlanDto> CreateAsync(CreateTrainingPlanDto dto, string userId)
    {
        var p = new TrainingPlan { UserId = userId, Title = dto.Title, Description = dto.Description, StartDate = dto.StartDate, EndDate = dto.EndDate };
        _db.TrainingPlans.Add(p);
        await _db.SaveChangesAsync();
        return new TrainingPlanDto { Id = p.Id, Title = p.Title, Description = p.Description, StartDate = p.StartDate, EndDate = p.EndDate, IsActive = p.IsActive, CreatedAt = p.CreatedAt };
    }

    public async Task<TrainingPlanDto?> UpdateAsync(int id, CreateTrainingPlanDto dto, string userId)
    {
        var p = await _db.TrainingPlans.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (p == null) return null;
        p.Title = dto.Title; p.Description = dto.Description; p.StartDate = dto.StartDate; p.EndDate = dto.EndDate;
        await _db.SaveChangesAsync();
        return new TrainingPlanDto { Id = p.Id, Title = p.Title, Description = p.Description, StartDate = p.StartDate, EndDate = p.EndDate, IsActive = p.IsActive };
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var p = await _db.TrainingPlans.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (p == null) return false;
        _db.TrainingPlans.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddExerciseAsync(int planId, int exerciseId, int dayOfWeek, int orderInDay, string userId)
    {
        var plan = await _db.TrainingPlans.FirstOrDefaultAsync(p => p.Id == planId && p.UserId == userId);
        if (plan == null) return false;
        _db.TrainingPlanExercises.Add(new TrainingPlanExercise { TrainingPlanId = planId, ExerciseId = exerciseId, DayOfWeek = dayOfWeek, OrderInDay = orderInDay });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveExerciseAsync(int planExerciseId, string userId)
    {
        var pe = await _db.TrainingPlanExercises.Include(x => x.TrainingPlan).FirstOrDefaultAsync(x => x.Id == planExerciseId && x.TrainingPlan.UserId == userId);
        if (pe == null) return false;
        _db.TrainingPlanExercises.Remove(pe);
        await _db.SaveChangesAsync();
        return true;
    }
}
