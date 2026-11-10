using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.DTOs.Exercise;

namespace NeuroPath.Core.Services;

public class ExerciseService : IExerciseService
{
    private const string CategoriesCacheKey = "exercise-categories";
    private const string DifficultyCacheKey = "difficulty-levels";

    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public ExerciseService(INeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<ExerciseDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.Exercises.AsNoTracking()
            .Where(e => e.IsActive)
            .OrderBy(e => e.Title)
            .Skip(skip).Take(take)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                DurationMinutes = e.DurationMinutes,
                Points = e.Points,
                CategoryName = e.ExerciseCategory.Name,
                DifficultyName = e.DifficultyLevel.Name,
                ImageUrl = e.ImageUrl,
                IsActive = e.IsActive,
                IsPremium = e.IsPremium,
                AverageRating = e.AverageRating,
                TotalCompletions = e.TotalCompletions
            })
            .ToListAsync();
    }

    public async Task<ExerciseDetailDto?> GetByIdAsync(int id, bool includeInactive = false)
        => await _db.Exercises.AsNoTracking()
            .Where(e => e.Id == id && (includeInactive || e.IsActive))
            .Select(e => new ExerciseDetailDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Instructions = e.Instructions,
                DurationMinutes = e.DurationMinutes,
                Points = e.Points,
                CategoryName = e.ExerciseCategory.Name,
                DifficultyName = e.DifficultyLevel.Name,
                ImageUrl = e.ImageUrl,
                VideoUrl = e.VideoUrl,
                IsActive = e.IsActive,
                IsPremium = e.IsPremium,
                AverageRating = e.AverageRating,
                TotalCompletions = e.TotalCompletions,
                ExerciseCategoryId = e.ExerciseCategoryId,
                DifficultyLevelId = e.DifficultyLevelId,
                Hints = e.Hints.OrderBy(h => h.HintOrder).Select(h => new ExerciseHintDto
                {
                    Id = h.Id,
                    HintText = h.HintText,
                    HintOrder = h.HintOrder,
                    PointPenalty = h.PointPenalty
                }).ToList(),
                BrainAreas = e.ExerciseBrainAreas.Select(b => new ExerciseBrainAreaInfo
                {
                    BrainAreaId = b.BrainAreaId,
                    BrainAreaName = b.BrainArea.Name,
                    ImpactLevel = b.ImpactLevel,
                    IsPrimary = b.IsPrimary
                }).ToList()
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync();

    public async Task<ExerciseDto> CreateAsync(CreateExerciseDto dto, string userId)
    {
        await EnsureLookupsExistAsync(dto.ExerciseCategoryId, dto.DifficultyLevelId);
        var exercise = new Exercise { CreatedByUserId = userId };
        Apply(exercise, dto);
        _db.Exercises.Add(exercise);
        await _db.SaveChangesAsync();
        return await ToDtoAsync(exercise.Id);
    }

    public async Task<ExerciseDto?> UpdateAsync(int id, UpdateExerciseDto dto, string userId, bool isAdmin)
    {
        var exercise = await _db.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null) return null;
        EnsureCanModify(exercise, userId, isAdmin);
        await EnsureLookupsExistAsync(dto.ExerciseCategoryId, dto.DifficultyLevelId);
        Apply(exercise, dto);
        exercise.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return await ToDtoAsync(exercise.Id);
    }

    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var exercise = await _db.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null) return false;
        EnsureCanModify(exercise, userId, isAdmin);
        exercise.IsActive = false;
        exercise.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ExerciseHintDto>> GetHintsAsync(int exerciseId)
        => await _db.ExerciseHints.AsNoTracking()
            .Where(h => h.ExerciseId == exerciseId)
            .OrderBy(h => h.HintOrder)
            .Select(h => new ExerciseHintDto { Id = h.Id, HintText = h.HintText, HintOrder = h.HintOrder, PointPenalty = h.PointPenalty })
            .ToListAsync();

    public async Task<ExerciseHintDto> AddHintAsync(int exerciseId, CreateExerciseHintDto dto, string userId, bool isAdmin)
    {
        var exercise = await _db.Exercises.AsNoTracking().FirstOrDefaultAsync(e => e.Id == exerciseId)
            ?? throw new NotFoundException("Exercise not found.");
        EnsureCanModify(exercise, userId, isAdmin);

        var hint = new ExerciseHint { ExerciseId = exerciseId, HintText = dto.HintText, HintOrder = dto.HintOrder, PointPenalty = dto.PointPenalty };
        _db.ExerciseHints.Add(hint);
        await _db.SaveChangesAsync();
        return new ExerciseHintDto { Id = hint.Id, HintText = hint.HintText, HintOrder = hint.HintOrder, PointPenalty = hint.PointPenalty };
    }

    public async Task<ExerciseHintDto?> UpdateHintAsync(int exerciseId, int hintId, CreateExerciseHintDto dto, string userId, bool isAdmin)
    {
        var exercise = await _db.Exercises.AsNoTracking().FirstOrDefaultAsync(e => e.Id == exerciseId)
            ?? throw new NotFoundException("Exercise not found.");
        EnsureCanModify(exercise, userId, isAdmin);

        var hint = await _db.ExerciseHints.FirstOrDefaultAsync(h => h.Id == hintId && h.ExerciseId == exerciseId);
        if (hint == null) return null;
        hint.HintText = dto.HintText;
        hint.HintOrder = dto.HintOrder;
        hint.PointPenalty = dto.PointPenalty;
        await _db.SaveChangesAsync();
        return new ExerciseHintDto { Id = hint.Id, HintText = hint.HintText, HintOrder = hint.HintOrder, PointPenalty = hint.PointPenalty };
    }

    public async Task<bool> DeleteHintAsync(int exerciseId, int hintId, string userId, bool isAdmin)
    {
        var exercise = await _db.Exercises.AsNoTracking().FirstOrDefaultAsync(e => e.Id == exerciseId);
        if (exercise == null) return false;
        EnsureCanModify(exercise, userId, isAdmin);

        var hint = await _db.ExerciseHints.FirstOrDefaultAsync(h => h.Id == hintId && h.ExerciseId == exerciseId);
        if (hint == null) return false;
        _db.ExerciseHints.Remove(hint);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ExerciseReviewDto>> GetReviewsAsync(int exerciseId)
        => await _db.ExerciseReviews.AsNoTracking()
            .Where(r => r.ExerciseId == exerciseId && r.IsApproved)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ExerciseReviewDto { Id = r.Id, UserName = r.User.UserName!, Rating = r.Rating, ReviewText = r.ReviewText, CreatedAt = r.CreatedAt })
            .ToListAsync();

    public async Task<ExerciseReviewDto> AddReviewAsync(int exerciseId, CreateExerciseReviewDto dto, string userId)
    {
        if (!await _db.Exercises.AnyAsync(e => e.Id == exerciseId && e.IsActive))
            throw new NotFoundException("Exercise not found.");
        if (!await _db.UserExerciseLogs.AnyAsync(l => l.UserId == userId && l.ExerciseId == exerciseId))
            throw new BusinessRuleException("You can only review exercises you have completed.");
        if (await _db.ExerciseReviews.AnyAsync(r => r.ExerciseId == exerciseId && r.UserId == userId))
            throw new ConflictException("You have already reviewed this exercise.");

        var review = new ExerciseReview { ExerciseId = exerciseId, UserId = userId, Rating = dto.Rating, ReviewText = dto.ReviewText };
        _db.ExerciseReviews.Add(review);
        await _db.SaveChangesAsync();

        var userName = await _db.Users.Where(u => u.Id == userId).Select(u => u.UserName).FirstOrDefaultAsync();
        return new ExerciseReviewDto { Id = review.Id, UserName = userName ?? string.Empty, Rating = review.Rating, ReviewText = review.ReviewText, CreatedAt = review.CreatedAt };
    }

    public async Task<bool> ApproveReviewAsync(int reviewId)
    {
        var review = await _db.ExerciseReviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null) return false;
        if (review.IsApproved) return true;

        review.IsApproved = true;
        await _db.SaveChangesAsync();

        var average = await _db.ExerciseReviews
            .Where(r => r.ExerciseId == review.ExerciseId && r.IsApproved)
            .AverageAsync(r => (decimal?)r.Rating) ?? 0m;
        var rounded = Math.Round(average, 2);
        await _db.Exercises.Where(e => e.Id == review.ExerciseId)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.AverageRating, rounded));
        return true;
    }

    public async Task<List<ExerciseCategoryDto>> GetCategoriesAsync()
    {
        if (_cache.TryGet<List<ExerciseCategoryDto>>(CategoriesCacheKey, out var cached) && cached != null) return cached;
        var list = await _db.ExerciseCategories.AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new ExerciseCategoryDto { Id = c.Id, Name = c.Name, Description = c.Description, IconUrl = c.IconUrl, Color = c.Color })
            .ToListAsync();
        _cache.Set(CategoriesCacheKey, list, TimeSpan.FromMinutes(60));
        return list;
    }

    public async Task<List<DifficultyLevelDto>> GetDifficultyLevelsAsync()
    {
        if (_cache.TryGet<List<DifficultyLevelDto>>(DifficultyCacheKey, out var cached) && cached != null) return cached;
        var list = await _db.DifficultyLevels.AsNoTracking()
            .OrderBy(d => d.Level)
            .Select(d => new DifficultyLevelDto { Id = d.Id, Name = d.Name, Level = d.Level, PointMultiplier = d.PointMultiplier, Color = d.Color })
            .ToListAsync();
        _cache.Set(DifficultyCacheKey, list, TimeSpan.FromMinutes(120));
        return list;
    }

    private async Task<ExerciseDto> ToDtoAsync(int id)
        => await _db.Exercises.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                DurationMinutes = e.DurationMinutes,
                Points = e.Points,
                CategoryName = e.ExerciseCategory.Name,
                DifficultyName = e.DifficultyLevel.Name,
                ImageUrl = e.ImageUrl,
                IsActive = e.IsActive,
                IsPremium = e.IsPremium,
                AverageRating = e.AverageRating,
                TotalCompletions = e.TotalCompletions
            })
            .FirstAsync();

    private static void Apply(Exercise exercise, CreateExerciseDto dto)
    {
        exercise.Title = dto.Title;
        exercise.Description = dto.Description;
        exercise.Instructions = dto.Instructions;
        exercise.DurationMinutes = dto.DurationMinutes;
        exercise.Points = dto.Points;
        exercise.ExerciseCategoryId = dto.ExerciseCategoryId;
        exercise.DifficultyLevelId = dto.DifficultyLevelId;
        exercise.ImageUrl = dto.ImageUrl;
        exercise.VideoUrl = dto.VideoUrl;
        exercise.IsActive = dto.IsActive;
        exercise.IsPremium = dto.IsPremium;
    }

    private static void EnsureCanModify(Exercise exercise, string userId, bool isAdmin)
    {
        if (!isAdmin && exercise.CreatedByUserId != userId)
            throw new ForbiddenException("Only the creator or an administrator can modify this exercise.");
    }

    private async Task EnsureLookupsExistAsync(int categoryId, int difficultyLevelId)
    {
        if (!await _db.ExerciseCategories.AnyAsync(c => c.Id == categoryId))
            throw new BusinessRuleException($"Exercise category {categoryId} does not exist.");
        if (!await _db.DifficultyLevels.AnyAsync(d => d.Id == difficultyLevelId))
            throw new BusinessRuleException($"Difficulty level {difficultyLevelId} does not exist.");
    }
}
