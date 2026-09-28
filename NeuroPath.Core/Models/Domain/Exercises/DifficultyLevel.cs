using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Core.Models.Domain.Courses;

namespace NeuroPath.Core.Models.Domain.Exercises;

public class DifficultyLevel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string? Description { get; set; }
    public decimal PointMultiplier { get; set; } = 1.0m;
    public string? Color { get; set; }

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();
}
