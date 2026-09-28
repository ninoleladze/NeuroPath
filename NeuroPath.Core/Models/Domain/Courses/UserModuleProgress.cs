using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Courses;

public class UserModuleProgress
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CourseModuleId { get; set; }
    public int? CurrentLessonId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public decimal ProgressPercent { get; set; }
    public int EarnedPoints { get; set; }

    public User User { get; set; } = null!;
    public CourseModule CourseModule { get; set; } = null!;
    public ModuleLesson? CurrentLesson { get; set; }
}
