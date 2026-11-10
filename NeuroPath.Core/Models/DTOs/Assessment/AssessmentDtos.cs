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
    public int? RequiredCourseModuleId { get; set; }
    public int? RequiredAssessmentId { get; set; }
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

public class SkillAssessmentDetailDto : SkillAssessmentDto
{
    public List<AssessmentQuestionDto> Questions { get; set; } = new();
}

public class AssessmentQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "SingleChoice";
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    public List<AssessmentAnswerDto> Answers { get; set; } = new();
}

public class AssessmentAnswerDto
{
    public int Id { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class CreateAssessmentQuestionDto
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "SingleChoice";
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    public List<CreateAssessmentAnswerDto> Answers { get; set; } = new();
}

public class CreateAssessmentAnswerDto
{
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}

public class SubmitAssessmentDto
{
    public List<AssessmentAnswerSubmission> Answers { get; set; } = new();
    public int TimeTakenSeconds { get; set; }
}

public class AssessmentAnswerSubmission
{
    public int QuestionId { get; set; }
    public int? SelectedAnswerId { get; set; }
}

public class CreateCertificateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CertificateType CertificateType { get; set; }
    public string? ImageUrl { get; set; }
    public int? RequiredCourseModuleId { get; set; }
    public int? RequiredAssessmentId { get; set; }
    public int? RequiredScorePercent { get; set; }
}
