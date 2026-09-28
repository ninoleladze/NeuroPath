using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Courses;
using NeuroPath.Core.Models.DTOs.Course;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class CourseModuleService : ICourseModuleService
{
    private readonly NeuroPathDbContext _db;

    public CourseModuleService(NeuroPathDbContext db) => _db = db;

    public async Task<List<CourseModuleDto>> GetAllAsync()
        => await _db.CourseModules.Where(c => c.IsPublished)
            .Include(c => c.DifficultyLevel).Include(c => c.Lessons)
            .Select(c => new CourseModuleDto { Id = c.Id, Title = c.Title, Description = c.Description, ShortDescription = c.ShortDescription, ImageUrl = c.ImageUrl, DifficultyName = c.DifficultyLevel.Name, DurationWeeks = c.DurationWeeks, TotalPoints = c.TotalPoints, IsPublished = c.IsPublished, IsFeatured = c.IsFeatured, LessonCount = c.Lessons.Count, CreatedAt = c.CreatedAt })
            .ToListAsync();

    public async Task<CourseModuleDetailDto?> GetByIdAsync(int id)
    {
        var c = await _db.CourseModules.Include(x => x.DifficultyLevel).Include(x => x.Lessons).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return null;
        return new CourseModuleDetailDto { Id = c.Id, Title = c.Title, Description = c.Description, ShortDescription = c.ShortDescription, ImageUrl = c.ImageUrl, DifficultyLevelId = c.DifficultyLevelId, DurationWeeks = c.DurationWeeks, TotalPoints = c.TotalPoints, IsPublished = c.IsPublished, IsFeatured = c.IsFeatured, Lessons = c.Lessons.OrderBy(l => l.OrderIndex).Select(l => new ModuleLessonDto { Id = l.Id, Title = l.Title, Content = l.Content, VideoUrl = l.VideoUrl, DurationMinutes = l.DurationMinutes, OrderIndex = l.OrderIndex, Points = l.Points }).ToList() };
    }

    public async Task<CourseModuleDto> CreateAsync(CreateCourseModuleDto dto, string userId)
    {
        var c = new CourseModule { Title = dto.Title, Description = dto.Description, ShortDescription = dto.ShortDescription, ImageUrl = dto.ImageUrl, DifficultyLevelId = dto.DifficultyLevelId, DurationWeeks = dto.DurationWeeks, TotalPoints = dto.TotalPoints, CreatedByUserId = userId };
        _db.CourseModules.Add(c);
        await _db.SaveChangesAsync();
        await _db.Entry(c).Reference(x => x.DifficultyLevel).LoadAsync();
        return new CourseModuleDto { Id = c.Id, Title = c.Title, Description = c.Description, ShortDescription = c.ShortDescription, ImageUrl = c.ImageUrl, DifficultyName = c.DifficultyLevel.Name, DurationWeeks = c.DurationWeeks, TotalPoints = c.TotalPoints, IsPublished = c.IsPublished, CreatedAt = c.CreatedAt };
    }

    public async Task<CourseModuleDto?> UpdateAsync(int id, CreateCourseModuleDto dto)
    {
        var c = await _db.CourseModules.Include(x => x.DifficultyLevel).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return null;
        c.Title = dto.Title; c.Description = dto.Description; c.ShortDescription = dto.ShortDescription;
        c.ImageUrl = dto.ImageUrl; c.DifficultyLevelId = dto.DifficultyLevelId; c.DurationWeeks = dto.DurationWeeks; c.TotalPoints = dto.TotalPoints;
        await _db.SaveChangesAsync();
        await _db.Entry(c).Reference(x => x.DifficultyLevel).LoadAsync();
        return new CourseModuleDto { Id = c.Id, Title = c.Title, Description = c.Description, DifficultyName = c.DifficultyLevel.Name, DurationWeeks = c.DurationWeeks, TotalPoints = c.TotalPoints, IsPublished = c.IsPublished };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var c = await _db.CourseModules.FindAsync(id);
        if (c == null) return false;
        _db.CourseModules.Remove(c);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<UserModuleProgressDto> EnrollAsync(int moduleId, string userId)
    {
        var existing = await _db.UserModuleProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.CourseModuleId == moduleId);
        if (existing != null)
            return new UserModuleProgressDto { Id = existing.Id, CourseModuleId = moduleId, StartedAt = existing.StartedAt, CompletedLessons = existing.CompletedLessons, TotalLessons = existing.TotalLessons, ProgressPercent = existing.ProgressPercent };

        var module = await _db.CourseModules.Include(x => x.Lessons).FirstOrDefaultAsync(x => x.Id == moduleId) ?? throw new InvalidOperationException("Module not found.");
        var progress = new UserModuleProgress { UserId = userId, CourseModuleId = moduleId, TotalLessons = module.Lessons.Count };
        _db.UserModuleProgresses.Add(progress);
        await _db.SaveChangesAsync();
        return new UserModuleProgressDto { Id = progress.Id, CourseModuleId = moduleId, CourseTitle = module.Title, StartedAt = progress.StartedAt, TotalLessons = progress.TotalLessons };
    }

    public async Task<UserModuleProgressDto?> UpdateProgressAsync(int moduleId, string userId, int lessonId)
    {
        var progress = await _db.UserModuleProgresses.Include(p => p.CourseModule).FirstOrDefaultAsync(p => p.UserId == userId && p.CourseModuleId == moduleId);
        if (progress == null) return null;
        progress.CurrentLessonId = lessonId; progress.CompletedLessons++;
        if (progress.TotalLessons > 0) progress.ProgressPercent = (decimal)progress.CompletedLessons / progress.TotalLessons * 100;
        if (progress.CompletedLessons >= progress.TotalLessons) progress.CompletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UserModuleProgressDto { Id = progress.Id, CourseModuleId = moduleId, CourseTitle = progress.CourseModule.Title, CurrentLessonId = progress.CurrentLessonId, StartedAt = progress.StartedAt, CompletedAt = progress.CompletedAt, CompletedLessons = progress.CompletedLessons, TotalLessons = progress.TotalLessons, ProgressPercent = progress.ProgressPercent };
    }

    public async Task<List<UserModuleProgressDto>> GetUserProgressAsync(string userId)
        => await _db.UserModuleProgresses.Where(p => p.UserId == userId)
            .Include(p => p.CourseModule)
            .Select(p => new UserModuleProgressDto { Id = p.Id, CourseModuleId = p.CourseModuleId, CourseTitle = p.CourseModule.Title, StartedAt = p.StartedAt, CompletedAt = p.CompletedAt, CompletedLessons = p.CompletedLessons, TotalLessons = p.TotalLessons, ProgressPercent = p.ProgressPercent, EarnedPoints = p.EarnedPoints })
            .ToListAsync();
}
