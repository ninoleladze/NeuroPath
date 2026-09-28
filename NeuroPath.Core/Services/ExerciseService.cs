using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.DTOs.Exercise;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class ExerciseService : IExerciseService
{
    private readonly NeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public ExerciseService(NeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<ExerciseDto>> GetAllAsync()
        => await _db.Exercises.Where(e => e.IsActive)
            .Include(e => e.ExerciseCategory).Include(e => e.DifficultyLevel)
            .Select(e => new ExerciseDto
            {
                Id = e.Id, Title = e.Title, Description = e.Description, DurationMinutes = e.DurationMinutes,
                Points = e.Points, CategoryName = e.ExerciseCategory.Name, DifficultyName = e.DifficultyLevel.Name,
                ImageUrl = e.ImageUrl, IsActive = e.IsActive, IsPremium = e.IsPremium,
                AverageRating = e.AverageRating, TotalCompletions = e.TotalCompletions
            })
            .ToListAsync();

    public async Task<ExerciseDetailDto?> GetByIdAsync(int id)
    {
        var e = await _db.Exercises.Include(x => x.ExerciseCategory).Include(x => x.DifficultyLevel)
            .Include(x => x.Hints).Include(x => x.ExerciseBrainAreas).ThenInclude(x => x.BrainArea)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return null;
        return new ExerciseDetailDto
        {
            Id = e.Id, Title = e.Title, Description = e.Description, Instructions = e.Instructions,
            DurationMinutes = e.DurationMinutes, Points = e.Points, CategoryName = e.ExerciseCategory.Name,
            DifficultyName = e.DifficultyLevel.Name, ImageUrl = e.ImageUrl, VideoUrl = e.VideoUrl,
            IsActive = e.IsActive, IsPremium = e.IsPremium, AverageRating = e.AverageRating,
            TotalCompletions = e.TotalCompletions, ExerciseCategoryId = e.ExerciseCategoryId,
            DifficultyLevelId = e.DifficultyLevelId,
            Hints = e.Hints.OrderBy(h => h.HintOrder).Select(h => new ExerciseHintDto
            {
                Id = h.Id, HintText = h.HintText, HintOrder = h.HintOrder, PointPenalty = h.PointPenalty
            }).ToList(),
            BrainAreas = e.ExerciseBrainAreas.Select(b => new ExerciseBrainAreaInfo
            {
                BrainAreaId = b.BrainAreaId, BrainAreaName = b.BrainArea.Name,
                ImpactLevel = b.ImpactLevel, IsPrimary = b.IsPrimary
            }).ToList()
        };
    }

    public async Task<ExerciseDto> CreateAsync(CreateExerciseDto dto, string userId)
    {
        var e = new Exercise
        {
            Title = dto.Title, Description = dto.Description, Instructions = dto.Instructions,
            DurationMinutes = dto.DurationMinutes, Points = dto.Points,
            ExerciseCategoryId = dto.ExerciseCategoryId, DifficultyLevelId = dto.DifficultyLevelId,
            ImageUrl = dto.ImageUrl, VideoUrl = dto.VideoUrl, IsActive = dto.IsActive,
            IsPremium = dto.IsPremium, CreatedByUserId = userId
        };
        _db.Exercises.Add(e);
        await _db.SaveChangesAsync();
        await _db.Entry(e).Reference(x => x.ExerciseCategory).LoadAsync();
        await _db.Entry(e).Reference(x => x.DifficultyLevel).LoadAsync();
        return new ExerciseDto
        {
            Id = e.Id, Title = e.Title, Description = e.Description, DurationMinutes = e.DurationMinutes,
            Points = e.Points, CategoryName = e.ExerciseCategory.Name, DifficultyName = e.DifficultyLevel.Name,
            ImageUrl = e.ImageUrl, IsActive = e.IsActive, IsPremium = e.IsPremium
        };
    }

    public async Task<ExerciseDto?> UpdateAsync(int id, UpdateExerciseDto dto)
    {
        var e = await _db.Exercises.Include(x => x.ExerciseCategory).Include(x => x.DifficultyLevel)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return null;
        e.Title = dto.Title; e.Description = dto.Description; e.Instructions = dto.Instructions;
        e.DurationMinutes = dto.DurationMinutes; e.Points = dto.Points;
        e.ExerciseCategoryId = dto.ExerciseCategoryId; e.DifficultyLevelId = dto.DifficultyLevelId;
        e.ImageUrl = dto.ImageUrl; e.VideoUrl = dto.VideoUrl;
        e.IsActive = dto.IsActive; e.IsPremium = dto.IsPremium; e.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _db.Entry(e).Reference(x => x.ExerciseCategory).LoadAsync();
        await _db.Entry(e).Reference(x => x.DifficultyLevel).LoadAsync();
        return new ExerciseDto
        {
            Id = e.Id, Title = e.Title, Description = e.Description, DurationMinutes = e.DurationMinutes,
            Points = e.Points, CategoryName = e.ExerciseCategory.Name, DifficultyName = e.DifficultyLevel.Name,
            ImageUrl = e.ImageUrl, IsActive = e.IsActive, IsPremium = e.IsPremium
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.Exercises.FindAsync(id);
        if (e == null) return false;
        _db.Exercises.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ExerciseHintDto>> GetHintsAsync(int exerciseId)
        => await _db.ExerciseHints.Where(h => h.ExerciseId == exerciseId).OrderBy(h => h.HintOrder)
            .Select(h => new ExerciseHintDto { Id = h.Id, HintText = h.HintText, HintOrder = h.HintOrder, PointPenalty = h.PointPenalty })
            .ToListAsync();

    public async Task<ExerciseHintDto> AddHintAsync(int exerciseId, CreateExerciseHintDto dto)
    {
        var h = new ExerciseHint { ExerciseId = exerciseId, HintText = dto.HintText, HintOrder = dto.HintOrder, PointPenalty = dto.PointPenalty };
        _db.ExerciseHints.Add(h);
        await _db.SaveChangesAsync();
        return new ExerciseHintDto { Id = h.Id, HintText = h.HintText, HintOrder = h.HintOrder, PointPenalty = h.PointPenalty };
    }

    public async Task<List<ExerciseReviewDto>> GetReviewsAsync(int exerciseId)
        => await _db.ExerciseReviews.Where(r => r.ExerciseId == exerciseId && r.IsApproved)
            .Include(r => r.User).OrderByDescending(r => r.CreatedAt)
            .Select(r => new ExerciseReviewDto { Id = r.Id, UserName = r.User.UserName!, Rating = r.Rating, ReviewText = r.ReviewText, CreatedAt = r.CreatedAt })
            .ToListAsync();

    public async Task<ExerciseReviewDto> AddReviewAsync(int exerciseId, CreateExerciseReviewDto dto, string userId)
    {
        var r = new ExerciseReview { ExerciseId = exerciseId, UserId = userId, Rating = dto.Rating, ReviewText = dto.ReviewText };
        _db.ExerciseReviews.Add(r);
        await _db.SaveChangesAsync();
        var avg = await _db.ExerciseReviews.Where(x => x.ExerciseId == exerciseId && x.IsApproved).AverageAsync(x => (double?)x.Rating) ?? 0;
        var exercise = await _db.Exercises.FindAsync(exerciseId);
        if (exercise != null) { exercise.AverageRating = (decimal)avg; await _db.SaveChangesAsync(); }
        var user = await _db.Users.FindAsync(userId);
        return new ExerciseReviewDto { Id = r.Id, UserName = user?.UserName ?? userId, Rating = r.Rating, ReviewText = r.ReviewText, CreatedAt = r.CreatedAt };
    }

    public async Task<List<ExerciseCategoryDto>> GetCategoriesAsync()
    {
        const string key = "exercise-categories";
        if (_cache.TryGet<List<ExerciseCategoryDto>>(key, out var c) && c != null) return c;
        var list = await _db.ExerciseCategories
            .Select(x => new ExerciseCategoryDto { Id = x.Id, Name = x.Name, Description = x.Description, IconUrl = x.IconUrl, Color = x.Color })
            .ToListAsync();
        _cache.Set(key, list, TimeSpan.FromMinutes(60));
        return list;
    }

    public async Task<List<DifficultyLevelDto>> GetDifficultyLevelsAsync()
    {
        const string key = "difficulty-levels";
        if (_cache.TryGet<List<DifficultyLevelDto>>(key, out var c) && c != null) return c;
        var list = await _db.DifficultyLevels.OrderBy(d => d.Level)
            .Select(x => new DifficultyLevelDto { Id = x.Id, Name = x.Name, Level = x.Level, PointMultiplier = x.PointMultiplier, Color = x.Color })
            .ToListAsync();
        _cache.Set(key, list, TimeSpan.FromMinutes(120));
        return list;
    }
}
