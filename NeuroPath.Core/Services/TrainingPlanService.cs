using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Core.Models.DTOs.Training;

namespace NeuroPath.Core.Services;

public class TrainingPlanService : ITrainingPlanService
{
    private readonly INeuroPathDbContext _db;

    public TrainingPlanService(INeuroPathDbContext db) => _db = db;

    public async Task<List<TrainingPlanDto>> GetUserPlansAsync(string userId)
        => await _db.TrainingPlans.AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.IsActive).ThenByDescending(p => p.StartDate)
            .Select(p => new TrainingPlanDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                ExerciseCount = p.Exercises.Count
            })
            .ToListAsync();

    public async Task<TrainingPlanDto?> GetByIdAsync(int id, string userId)
        => await _db.TrainingPlans.AsNoTracking()
            .Where(p => p.Id == id && p.UserId == userId)
            .Select(p => new TrainingPlanDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                ExerciseCount = p.Exercises.Count
            })
            .FirstOrDefaultAsync();

    public async Task<TrainingPlanDto> CreateAsync(CreateTrainingPlanDto dto, string userId)
    {
        var plan = new TrainingPlan
        {
            UserId = userId,
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate.Date,
            EndDate = dto.EndDate.Date
        };
        _db.TrainingPlans.Add(plan);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(plan.Id, userId))!;
    }

    public async Task<TrainingPlanDto?> UpdateAsync(int id, CreateTrainingPlanDto dto, string userId)
    {
        var plan = await _db.TrainingPlans.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        if (plan == null) return null;
        plan.Title = dto.Title;
        plan.Description = dto.Description;
        plan.StartDate = dto.StartDate.Date;
        plan.EndDate = dto.EndDate.Date;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id, userId);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var plan = await _db.TrainingPlans.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        if (plan == null) return false;
        _db.TrainingPlans.Remove(plan);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddExerciseAsync(int planId, AddPlanExerciseDto dto, string userId)
    {
        if (!await _db.TrainingPlans.AnyAsync(p => p.Id == planId && p.UserId == userId))
            return false;
        if (!await _db.Exercises.AnyAsync(e => e.Id == dto.ExerciseId && e.IsActive))
            throw new NotFoundException("Exercise not found.");

        _db.TrainingPlanExercises.Add(new TrainingPlanExercise
        {
            TrainingPlanId = planId,
            ExerciseId = dto.ExerciseId,
            DayOfWeek = dto.DayOfWeek,
            OrderInDay = dto.OrderInDay
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveExerciseAsync(int planExerciseId, string userId)
        => await _db.TrainingPlanExercises
            .Where(x => x.Id == planExerciseId && x.TrainingPlan.UserId == userId)
            .ExecuteDeleteAsync() > 0;
}
