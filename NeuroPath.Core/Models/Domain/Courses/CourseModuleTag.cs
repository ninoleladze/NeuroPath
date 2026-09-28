using NeuroPath.Core.Models.Domain.Content;

namespace NeuroPath.Core.Models.Domain.Courses;

public class CourseModuleTag
{
    public int CourseModuleId { get; set; }
    public int TagId { get; set; }

    public CourseModule CourseModule { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
