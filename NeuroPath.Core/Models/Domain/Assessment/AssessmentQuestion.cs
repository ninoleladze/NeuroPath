namespace NeuroPath.Core.Models.Domain.Assessment;

public class AssessmentQuestion
{
    public int Id { get; set; }
    public int SkillAssessmentId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "SingleChoice";
    public int Points { get; set; }
    public int OrderIndex { get; set; }

    public SkillAssessment SkillAssessment { get; set; } = null!;
    public ICollection<AssessmentAnswer> Answers { get; set; } = new List<AssessmentAnswer>();
}
