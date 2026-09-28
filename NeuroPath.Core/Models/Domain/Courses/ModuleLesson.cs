namespace NeuroPath.Core.Models.Domain.Courses;

public class ModuleLesson
{
    public int Id { get; set; }
    public int CourseModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public int DurationMinutes { get; set; }
    public int OrderIndex { get; set; }
    public int Points { get; set; }

    public CourseModule CourseModule { get; set; } = null!;
    public ICollection<LessonExercise> LessonExercises { get; set; } = new List<LessonExercise>();
}
