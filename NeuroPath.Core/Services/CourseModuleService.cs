using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Courses;
using NeuroPath.Core.Models.DTOs.Course;

namespace NeuroPath.Core.Services;

public class CourseModuleService : ICourseModuleService
{
    private readonly INeuroPathDbContext _db;
    private readonly IProgressionService _progression;
    private readonly ICertificateService _certificates;
    private readonly IAchievementService _achievements;

    public CourseModuleService(INeuroPathDbContext db, IProgressionService progression, ICertificateService certificates, IAchievementService achievements)
    {
        _db = db;
        _progression = progression;
        _certificates = certificates;
        _achievements = achievements;
    }

    public async Task<List<CourseModuleDto>> GetAllAsync()
        => await _db.CourseModules.AsNoTracking()
            .Where(c => c.IsPublished)
            .OrderBy(c => c.OrderIndex).ThenBy(c => c.Title)
            .Select(c => new CourseModuleDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ShortDescription = c.ShortDescription,
                ImageUrl = c.ImageUrl,
                DifficultyName = c.DifficultyLevel.Name,
                DurationWeeks = c.DurationWeeks,
                TotalPoints = c.TotalPoints,
                IsPublished = c.IsPublished,
                IsFeatured = c.IsFeatured,
                LessonCount = c.Lessons.Count,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

    public async Task<CourseModuleDetailDto?> GetByIdAsync(int id, bool includeUnpublished = false)
        => await _db.CourseModules.AsNoTracking()
            .Where(c => c.Id == id && (includeUnpublished || c.IsPublished))
            .Select(c => new CourseModuleDetailDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ShortDescription = c.ShortDescription,
                ImageUrl = c.ImageUrl,
                DifficultyLevelId = c.DifficultyLevelId,
                DurationWeeks = c.DurationWeeks,
                TotalPoints = c.TotalPoints,
                IsPublished = c.IsPublished,
                IsFeatured = c.IsFeatured,
                Lessons = c.Lessons.OrderBy(l => l.OrderIndex).Select(l => new ModuleLessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Content = l.Content,
                    VideoUrl = l.VideoUrl,
                    DurationMinutes = l.DurationMinutes,
                    OrderIndex = l.OrderIndex,
                    Points = l.Points
                }).ToList()
            })
            .FirstOrDefaultAsync();

    public async Task<CourseModuleDto> CreateAsync(CreateCourseModuleDto dto, string userId)
    {
        await EnsureDifficultyExistsAsync(dto.DifficultyLevelId);
        var module = new CourseModule { CreatedByUserId = userId };
        Apply(module, dto);
        _db.CourseModules.Add(module);
        await _db.SaveChangesAsync();
        return await ToDtoAsync(module.Id);
    }

    public async Task<CourseModuleDto?> UpdateAsync(int id, CreateCourseModuleDto dto, string userId, bool isAdmin)
    {
        var module = await _db.CourseModules.FirstOrDefaultAsync(c => c.Id == id);
        if (module == null) return null;
        EnsureCanModify(module, userId, isAdmin);
        await EnsureDifficultyExistsAsync(dto.DifficultyLevelId);
        Apply(module, dto);
        await _db.SaveChangesAsync();
        return await ToDtoAsync(module.Id);
    }

    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var module = await _db.CourseModules.FirstOrDefaultAsync(c => c.Id == id);
        if (module == null) return false;
        EnsureCanModify(module, userId, isAdmin);

        var hasLearners = await _db.UserModuleProgresses.AnyAsync(p => p.CourseModuleId == id);
        var isRequiredByCertificate = await _db.Certificates.AnyAsync(c => c.RequiredCourseModuleId == id);
        if (hasLearners || isRequiredByCertificate)
        {
            module.IsPublished = false;
            module.IsFeatured = false;
        }
        else
        {
            _db.CourseModules.Remove(module);
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PublishAsync(int id, string userId, bool isAdmin)
    {
        var module = await _db.CourseModules.FirstOrDefaultAsync(c => c.Id == id);
        if (module == null) return false;
        EnsureCanModify(module, userId, isAdmin);
        if (!await _db.ModuleLessons.AnyAsync(l => l.CourseModuleId == id))
            throw new BusinessRuleException("A module needs at least one lesson before it can be published.");
        module.IsPublished = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<ModuleLessonDto> AddLessonAsync(int moduleId, CreateModuleLessonDto dto, string userId, bool isAdmin)
    {
        var module = await _db.CourseModules.FirstOrDefaultAsync(c => c.Id == moduleId)
            ?? throw new NotFoundException("Course module not found.");
        EnsureCanModify(module, userId, isAdmin);

        var lesson = new ModuleLesson
        {
            CourseModuleId = moduleId,
            Title = dto.Title,
            Content = dto.Content,
            VideoUrl = dto.VideoUrl,
            DurationMinutes = dto.DurationMinutes,
            OrderIndex = dto.OrderIndex,
            Points = dto.Points
        };

        await using var transaction = await _db.Database.BeginTransactionAsync();
        _db.ModuleLessons.Add(lesson);
        await _db.SaveChangesAsync();
        await _db.UserModuleProgresses
            .Where(p => p.CourseModuleId == moduleId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.TotalLessons, p => p.TotalLessons + 1));
        await transaction.CommitAsync();

        return ToLessonDto(lesson);
    }

    public async Task<ModuleLessonDto?> UpdateLessonAsync(int moduleId, int lessonId, CreateModuleLessonDto dto, string userId, bool isAdmin)
    {
        var module = await _db.CourseModules.FirstOrDefaultAsync(c => c.Id == moduleId)
            ?? throw new NotFoundException("Course module not found.");
        EnsureCanModify(module, userId, isAdmin);

        var lesson = await _db.ModuleLessons.FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseModuleId == moduleId);
        if (lesson == null) return null;
        if (lesson.Points != dto.Points && await _db.UserLessonCompletions.AnyAsync(c => c.ModuleLessonId == lessonId))
            throw new BusinessRuleException("Points cannot be changed after learners have completed this lesson.");

        lesson.Title = dto.Title;
        lesson.Content = dto.Content;
        lesson.VideoUrl = dto.VideoUrl;
        lesson.DurationMinutes = dto.DurationMinutes;
        lesson.OrderIndex = dto.OrderIndex;
        lesson.Points = dto.Points;
        await _db.SaveChangesAsync();
        return ToLessonDto(lesson);
    }

    public async Task<bool> DeleteLessonAsync(int moduleId, int lessonId, string userId, bool isAdmin)
    {
        var module = await _db.CourseModules.FirstOrDefaultAsync(c => c.Id == moduleId);
        if (module == null) return false;
        EnsureCanModify(module, userId, isAdmin);

        var lesson = await _db.ModuleLessons.FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseModuleId == moduleId);
        if (lesson == null) return false;
        if (await _db.UserLessonCompletions.AnyAsync(c => c.ModuleLessonId == lessonId))
            throw new ConflictException("Learners have already completed this lesson, so it cannot be deleted.");
        if (module.IsPublished && await _db.ModuleLessons.CountAsync(l => l.CourseModuleId == moduleId) == 1)
            throw new BusinessRuleException("A published module must keep at least one lesson.");

        await using var transaction = await _db.Database.BeginTransactionAsync();
        await _db.UserModuleProgresses
            .Where(p => p.CurrentLessonId == lessonId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.CurrentLessonId, (int?)null));
        await _db.UserModuleProgresses
            .Where(p => p.CourseModuleId == moduleId && p.TotalLessons > 0)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.TotalLessons, p => p.TotalLessons - 1));
        _db.ModuleLessons.Remove(lesson);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }

    public async Task<UserModuleProgressDto> EnrollAsync(int moduleId, string userId)
    {
        var existing = await _db.UserModuleProgresses.AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId && p.CourseModuleId == moduleId);
        if (existing != null) return await ToProgressDtoAsync(existing.Id);

        var module = await _db.CourseModules.AsNoTracking()
            .Where(c => c.Id == moduleId && c.IsPublished)
            .Select(c => new { c.Id, LessonCount = c.Lessons.Count, FirstLessonId = c.Lessons.OrderBy(l => l.OrderIndex).Select(l => (int?)l.Id).FirstOrDefault() })
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Course module not found.");

        var progress = new UserModuleProgress
        {
            UserId = userId,
            CourseModuleId = moduleId,
            TotalLessons = module.LessonCount,
            CurrentLessonId = module.FirstLessonId
        };
        _db.UserModuleProgresses.Add(progress);
        await _db.SaveChangesAsync();
        return await ToProgressDtoAsync(progress.Id);
    }

    public async Task<UserModuleProgressDto?> CompleteLessonAsync(int moduleId, int lessonId, string userId)
    {
        var progress = await _db.UserModuleProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.CourseModuleId == moduleId);
        if (progress == null) return null;

        var lesson = await _db.ModuleLessons.AsNoTracking().FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseModuleId == moduleId)
            ?? throw new NotFoundException("Lesson not found in this module.");

        if (await _db.UserLessonCompletions.AnyAsync(c => c.UserId == userId && c.ModuleLessonId == lessonId))
            return await ToProgressDtoAsync(progress.Id);

        var lessons = await _db.ModuleLessons.AsNoTracking()
            .Where(l => l.CourseModuleId == moduleId)
            .OrderBy(l => l.OrderIndex)
            .Select(l => l.Id)
            .ToListAsync();
        var completedBefore = await _db.UserLessonCompletions
            .Where(c => c.UserId == userId && c.ModuleLesson.CourseModuleId == moduleId)
            .Select(c => c.ModuleLessonId)
            .ToListAsync();
        var completed = completedBefore.Append(lessonId).ToHashSet();

        var justCompleted = false;
        await using (var transaction = await _db.Database.BeginTransactionAsync())
        {
            _db.UserLessonCompletions.Add(new UserLessonCompletion { UserId = userId, ModuleLessonId = lessonId });

            progress.TotalLessons = lessons.Count;
            progress.CompletedLessons = completed.Count;
            progress.EarnedPoints += lesson.Points;
            progress.ProgressPercent = lessons.Count == 0 ? 0 : Math.Round((decimal)completed.Count / lessons.Count * 100, 2);
            var nextLessonId = lessons.FirstOrDefault(id => !completed.Contains(id));
            progress.CurrentLessonId = nextLessonId != 0 ? nextLessonId : lessonId;

            if (progress.CompletedAt == null && completed.Count >= lessons.Count)
            {
                progress.CompletedAt = DateTime.UtcNow;
                justCompleted = true;
            }

            await _progression.AwardPointsAsync(userId, lesson.Points, PointSource.Lesson, lesson.Id);
            await _progression.RegisterActivityAsync(userId);
            await _progression.ApplyChallengeProgressAsync(userId, lesson.Points, countsAsExercise: false, accuracy: null);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        if (justCompleted)
            await _certificates.AwardForCourseAsync(userId, moduleId);
        await _achievements.CheckAndAwardAsync(userId);

        return await ToProgressDtoAsync(progress.Id);
    }

    public async Task<List<UserModuleProgressDto>> GetUserProgressAsync(string userId)
        => await _db.UserModuleProgresses.AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.StartedAt)
            .Select(p => new UserModuleProgressDto
            {
                Id = p.Id,
                CourseModuleId = p.CourseModuleId,
                CourseTitle = p.CourseModule.Title,
                CurrentLessonId = p.CurrentLessonId,
                StartedAt = p.StartedAt,
                CompletedAt = p.CompletedAt,
                CompletedLessons = p.CompletedLessons,
                TotalLessons = p.TotalLessons,
                ProgressPercent = p.ProgressPercent,
                EarnedPoints = p.EarnedPoints
            })
            .ToListAsync();

    private async Task<UserModuleProgressDto> ToProgressDtoAsync(int progressId)
        => await _db.UserModuleProgresses.AsNoTracking()
            .Where(p => p.Id == progressId)
            .Select(p => new UserModuleProgressDto
            {
                Id = p.Id,
                CourseModuleId = p.CourseModuleId,
                CourseTitle = p.CourseModule.Title,
                CurrentLessonId = p.CurrentLessonId,
                StartedAt = p.StartedAt,
                CompletedAt = p.CompletedAt,
                CompletedLessons = p.CompletedLessons,
                TotalLessons = p.TotalLessons,
                ProgressPercent = p.ProgressPercent,
                EarnedPoints = p.EarnedPoints
            })
            .FirstAsync();

    private async Task<CourseModuleDto> ToDtoAsync(int id)
        => await _db.CourseModules.AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseModuleDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ShortDescription = c.ShortDescription,
                ImageUrl = c.ImageUrl,
                DifficultyName = c.DifficultyLevel.Name,
                DurationWeeks = c.DurationWeeks,
                TotalPoints = c.TotalPoints,
                IsPublished = c.IsPublished,
                IsFeatured = c.IsFeatured,
                LessonCount = c.Lessons.Count,
                CreatedAt = c.CreatedAt
            })
            .FirstAsync();

    private static ModuleLessonDto ToLessonDto(ModuleLesson lesson) => new()
    {
        Id = lesson.Id,
        Title = lesson.Title,
        Content = lesson.Content,
        VideoUrl = lesson.VideoUrl,
        DurationMinutes = lesson.DurationMinutes,
        OrderIndex = lesson.OrderIndex,
        Points = lesson.Points
    };

    private static void Apply(CourseModule module, CreateCourseModuleDto dto)
    {
        module.Title = dto.Title;
        module.Description = dto.Description;
        module.ShortDescription = dto.ShortDescription;
        module.ImageUrl = dto.ImageUrl;
        module.DifficultyLevelId = dto.DifficultyLevelId;
        module.DurationWeeks = dto.DurationWeeks;
        module.TotalPoints = dto.TotalPoints;
    }

    private static void EnsureCanModify(CourseModule module, string userId, bool isAdmin)
    {
        if (!isAdmin && module.CreatedByUserId != userId)
            throw new ForbiddenException("Only the creator or an administrator can modify this module.");
    }

    private async Task EnsureDifficultyExistsAsync(int difficultyLevelId)
    {
        if (!await _db.DifficultyLevels.AnyAsync(d => d.Id == difficultyLevelId))
            throw new BusinessRuleException($"Difficulty level {difficultyLevelId} does not exist.");
    }
}
