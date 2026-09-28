using NeuroPath.Core.Enums;

namespace NeuroPath.Core.Models.DTOs.Assessment;

public class SkillAssessmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CognitiveSkillId { get; set; }
    public string CognitiveSkillName { get; set; } = string.Empty;
    public AssessmentType AssessmentType { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int TotalQuestions { get; set; }
    public int PassingScorePercent { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSkillAssessmentDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CognitiveSkillId { get; set; }
    public AssessmentType AssessmentType { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int PassingScorePercent { get; set; } = 60;
}

public class UserAssessmentResultDto
{
    public int Id { get; set; }
    public int SkillAssessmentId { get; set; }
    public string AssessmentTitle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public decimal ScorePercent { get; set; }
    public bool IsPassed { get; set; }
    public int TimeTakenSeconds { get; set; }
}

public class CertificateDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CertificateType CertificateType { get; set; }
    public string? ImageUrl { get; set; }
    public int? RequiredScorePercent { get; set; }
}

public class UserCertificateDto
{
    public int CertificateId { get; set; }
    public string CertificateTitle { get; set; } = string.Empty;
    public DateTime EarnedAt { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string? PdfUrl { get; set; }
}
