namespace NeuroPath.Core.Models.Domain.Assessment;

public class AssessmentAnswer
{
    public int Id { get; set; }
    public int AssessmentQuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }

    public AssessmentQuestion AssessmentQuestion { get; set; } = null!;
}
