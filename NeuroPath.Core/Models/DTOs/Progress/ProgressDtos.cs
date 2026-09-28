namespace NeuroPath.Core.Models.DTOs.Progress;

public class ProgressSummaryDto
{
    public int TotalPoints { get; set; }
    public int ExercisesCompleted { get; set; }
    public int QuizzesCompleted { get; set; }
    public int StreakDays { get; set; }
    public decimal AverageAccuracy { get; set; }
    public int TotalTrainingMinutes { get; set; }
    public string? TopBrainArea { get; set; }
    public string? WeakestBrainArea { get; set; }
}

public class BrainAreaBreakdownDto
{
    public int BrainAreaId { get; set; }
    public string BrainAreaName { get; set; } = string.Empty;
    public string? Color { get; set; }
    public int CurrentLevel { get; set; }
    public int ExperiencePoints { get; set; }
    public int TotalExercises { get; set; }
    public decimal AverageAccuracy { get; set; }
}
