using NeuroPath.Core.Enums;

namespace NeuroPath.Core.Models.DTOs.Training;

public class TrainingPlanDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ExerciseCount { get; set; }
}

public class CreateTrainingPlanDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class DailySessionDto
{
    public int Id { get; set; }
    public DateTime SessionDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int TotalPoints { get; set; }
    public int TotalExercises { get; set; }
    public SessionStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class CreateDailySessionDto
{
    public int? TrainingPlanId { get; set; }
    public DateTime SessionDate { get; set; }
    public string? Notes { get; set; }
}

public class UserExerciseLogDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string ExerciseTitle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public int Score { get; set; }
    public int DurationSeconds { get; set; }
    public decimal AccuracyPercent { get; set; }
    public int DifficultyAtCompletion { get; set; }
}

public class CreateUserExerciseLogDto
{
    public int ExerciseId { get; set; }
    public int Score { get; set; }
    public int DurationSeconds { get; set; }
    public decimal AccuracyPercent { get; set; }
    public int HintsUsed { get; set; }
}

public class ExerciseLogResultDto
{
    public int LogId { get; set; }
    public int PointsAwarded { get; set; }
    public int SessionTotalPoints { get; set; }
    public int SessionTotalExercises { get; set; }
}

public class SessionStatsDto
{
    public int TotalExercisesCompleted { get; set; }
    public decimal AverageAccuracy { get; set; }
    public int TotalTrainingMinutes { get; set; }
}

public class AddPlanExerciseDto
{
    public int ExerciseId { get; set; }
    public int DayOfWeek { get; set; }
    public int OrderInDay { get; set; }
}
