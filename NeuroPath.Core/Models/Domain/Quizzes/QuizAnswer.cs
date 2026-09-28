namespace NeuroPath.Core.Models.Domain.Quizzes;

public class QuizAnswer
{
    public int Id { get; set; }
    public int QuizQuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }

    public QuizQuestion QuizQuestion { get; set; } = null!;
}
