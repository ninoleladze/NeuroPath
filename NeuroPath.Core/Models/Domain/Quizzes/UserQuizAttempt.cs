using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Quizzes;

public class UserQuizAttempt
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public decimal ScorePercent { get; set; }
    public bool IsPassed { get; set; }
    public int AttemptNumber { get; set; } = 1;
    public int PointsAwarded { get; set; }

    public User User { get; set; } = null!;
    public Quiz Quiz { get; set; } = null!;
    public ICollection<UserQuizAnswer> UserAnswers { get; set; } = new List<UserQuizAnswer>();
}
