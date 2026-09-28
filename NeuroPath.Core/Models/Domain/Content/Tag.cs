using NeuroPath.Core.Models.Domain.Courses;
using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Core.Models.Domain.Content;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    public ICollection<ExerciseTag> ExerciseTags { get; set; } = new List<ExerciseTag>();
    public ICollection<CourseModuleTag> CourseModuleTags { get; set; } = new List<CourseModuleTag>();
}
