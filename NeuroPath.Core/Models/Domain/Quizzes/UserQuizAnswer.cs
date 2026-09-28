namespace NeuroPath.Core.Models.Domain.Quizzes;

public class UserQuizAnswer
{
    public int Id { get; set; }
    public int UserQuizAttemptId { get; set; }
    public int QuizQuestionId { get; set; }
    public int? SelectedAnswerId { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

    public UserQuizAttempt UserQuizAttempt { get; set; } = null!;
    public QuizQuestion QuizQuestion { get; set; } = null!;
    public QuizAnswer? SelectedAnswer { get; set; }
}
