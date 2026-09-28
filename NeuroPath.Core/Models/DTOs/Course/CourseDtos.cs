namespace NeuroPath.Core.Models.DTOs.Course;

public class CourseModuleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string DifficultyName { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }
    public int TotalPoints { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public int LessonCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CourseModuleDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int DifficultyLevelId { get; set; }
    public int DurationWeeks { get; set; }
    public int TotalPoints { get; set; }
    public bool IsPublished { get; set; }
    public bool IsFeatured { get; set; }
    public List<ModuleLessonDto> Lessons { get; set; } = new();
}

public class CreateCourseModuleDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int DifficultyLevelId { get; set; }
    public int DurationWeeks { get; set; }
    public int TotalPoints { get; set; }
}

public class ModuleLessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public int DurationMinutes { get; set; }
    public int OrderIndex { get; set; }
    public int Points { get; set; }
}

public class CreateModuleLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public int DurationMinutes { get; set; }
    public int OrderIndex { get; set; }
    public int Points { get; set; }
}

public class UserModuleProgressDto
{
    public int Id { get; set; }
    public int CourseModuleId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int? CurrentLessonId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public decimal ProgressPercent { get; set; }
    public int EarnedPoints { get; set; }
}
