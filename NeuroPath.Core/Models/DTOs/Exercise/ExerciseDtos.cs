namespace NeuroPath.Core.Models.DTOs.Exercise;

public class ExerciseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int Points { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string DifficultyName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsPremium { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalCompletions { get; set; }
}

public class ExerciseDetailDto : ExerciseDto
{
    public string Instructions { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public int ExerciseCategoryId { get; set; }
    public int DifficultyLevelId { get; set; }
    public List<ExerciseHintDto> Hints { get; set; } = new();
    public List<ExerciseBrainAreaInfo> BrainAreas { get; set; } = new();
}

public class CreateExerciseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int Points { get; set; }
    public int ExerciseCategoryId { get; set; }
    public int DifficultyLevelId { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
}

public class UpdateExerciseDto : CreateExerciseDto { }

public class ExerciseHintDto
{
    public int Id { get; set; }
    public string HintText { get; set; } = string.Empty;
    public int HintOrder { get; set; }
    public int PointPenalty { get; set; }
}

public class CreateExerciseHintDto
{
    public string HintText { get; set; } = string.Empty;
    public int HintOrder { get; set; }
    public int PointPenalty { get; set; }
}

public class ExerciseBrainAreaInfo
{
    public int BrainAreaId { get; set; }
    public string BrainAreaName { get; set; } = string.Empty;
    public int ImpactLevel { get; set; }
    public bool IsPrimary { get; set; }
}

public class ExerciseReviewDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateExerciseReviewDto
{
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
}

public class ExerciseCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? Color { get; set; }
}

public class DifficultyLevelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public decimal PointMultiplier { get; set; }
    public string? Color { get; set; }
}

public class ExerciseLogDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string ExerciseTitle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public int Score { get; set; }
    public int DurationSeconds { get; set; }
    public decimal AccuracyPercent { get; set; }
}
