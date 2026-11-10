using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Courses;

public class UserLessonCompletion
{
    public string UserId { get; set; } = string.Empty;
    public int ModuleLessonId { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public ModuleLesson ModuleLesson { get; set; } = null!;
}
