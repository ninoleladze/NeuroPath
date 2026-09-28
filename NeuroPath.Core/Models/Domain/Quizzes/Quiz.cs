using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Quizzes;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DifficultyLevelId { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public int PassingScorePercent { get; set; } = 60;
    public int TotalPoints { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPublished { get; set; } = false;
    public string CreatedByUserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DifficultyLevel DifficultyLevel { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public ICollection<UserQuizAttempt> Attempts { get; set; } = new List<UserQuizAttempt>();
}
