using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Core.Models.Domain.Courses;

public class LessonExercise
{
    public int Id { get; set; }
    public int ModuleLessonId { get; set; }
    public int ExerciseId { get; set; }
    public int OrderIndex { get; set; }
    public bool IsRequired { get; set; }

    public ModuleLesson ModuleLesson { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}
