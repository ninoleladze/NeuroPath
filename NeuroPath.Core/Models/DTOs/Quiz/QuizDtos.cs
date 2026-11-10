namespace NeuroPath.Core.Models.DTOs.Quiz;

public class QuizDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DifficultyName { get; set; } = string.Empty;
    public int? TimeLimitMinutes { get; set; }
    public int PassingScorePercent { get; set; }
    public int TotalPoints { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public int QuestionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class QuizDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DifficultyLevelId { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public int PassingScorePercent { get; set; }
    public int TotalPoints { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class CreateQuizDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DifficultyLevelId { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public int PassingScorePercent { get; set; } = 60;
}

public class QuizQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "SingleChoice";
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    public string? ImageUrl { get; set; }
    public List<QuizAnswerDto> Answers { get; set; } = new();
}

public class QuizAnswerDto
{
    public int Id { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class CreateQuizQuestionDto
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "SingleChoice";
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    public string? Explanation { get; set; }
    public string? ImageUrl { get; set; }
    public List<CreateQuizAnswerDto> Answers { get; set; } = new();
}

public class CreateQuizAnswerDto
{
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}

public class SubmitQuizDto
{
    public int QuizId { get; set; }
    public int? AttemptId { get; set; }
    public List<UserAnswerSubmission> Answers { get; set; } = new();
}

public class UserAnswerSubmission
{
    public int QuestionId { get; set; }
    public int? SelectedAnswerId { get; set; }
}

public class QuizAttemptResultDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public decimal ScorePercent { get; set; }
    public bool IsPassed { get; set; }
    public int AttemptNumber { get; set; }
    public int PointsAwarded { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<QuizAnswerResultDto> AnswerResults { get; set; } = new();
}

public class QuizAnswerResultDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int? SelectedAnswerId { get; set; }
    public bool IsCorrect { get; set; }
    public string? Explanation { get; set; }
}

public class QuizStartDto
{
    public int AttemptId { get; set; }
    public int QuizId { get; set; }
    public int AttemptNumber { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
