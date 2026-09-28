using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Courses;

public class CourseModule
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int DifficultyLevelId { get; set; }
    public int DurationWeeks { get; set; }
    public int TotalPoints { get; set; }
    public bool IsPublished { get; set; } = false;
    public bool IsFeatured { get; set; } = false;
    public int OrderIndex { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DifficultyLevel DifficultyLevel { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public ICollection<ModuleLesson> Lessons { get; set; } = new List<ModuleLesson>();
    public ICollection<CourseModuleTag> CourseModuleTags { get; set; } = new List<CourseModuleTag>();
    public ICollection<UserModuleProgress> UserProgresses { get; set; } = new List<UserModuleProgress>();
}
