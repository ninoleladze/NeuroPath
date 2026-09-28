using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Courses;

namespace NeuroPath.Core.Models.Domain.Assessment;

public class Certificate
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CertificateType CertificateType { get; set; }
    public string? ImageUrl { get; set; }
    public int? RequiredCourseModuleId { get; set; }
    public int? RequiredAssessmentId { get; set; }
    public int? RequiredScorePercent { get; set; }

    public CourseModule? RequiredCourseModule { get; set; }
    public SkillAssessment? RequiredAssessment { get; set; }
    public ICollection<UserCertificate> UserCertificates { get; set; } = new List<UserCertificate>();
}
