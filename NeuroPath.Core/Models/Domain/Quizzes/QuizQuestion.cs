namespace NeuroPath.Core.Models.Domain.Quizzes;

public class QuizQuestion
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "SingleChoice";
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    public string? Explanation { get; set; }
    public string? ImageUrl { get; set; }

    public Quiz Quiz { get; set; } = null!;
    public ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
    public ICollection<UserQuizAnswer> UserAnswers { get; set; } = new List<UserQuizAnswer>();
}
