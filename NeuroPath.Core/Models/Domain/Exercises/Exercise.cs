using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.Domain.Courses;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.Domain.Training;

namespace NeuroPath.Core.Models.Domain.Exercises;

public class Exercise
{
    public int Id { get; set; }
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
    public int TotalCompletions { get; set; } = 0;
    public decimal AverageRating { get; set; } = 0;
    public string? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ExerciseCategory ExerciseCategory { get; set; } = null!;
    public DifficultyLevel DifficultyLevel { get; set; } = null!;
    public User? CreatedByUser { get; set; }
    public ICollection<ExerciseMedia> ExerciseMedia { get; set; } = new List<ExerciseMedia>();
    public ICollection<ExerciseBrainArea> ExerciseBrainAreas { get; set; } = new List<ExerciseBrainArea>();
    public ICollection<ExerciseTag> ExerciseTags { get; set; } = new List<ExerciseTag>();
    public ICollection<ExercisePrerequisite> Prerequisites { get; set; } = new List<ExercisePrerequisite>();
    public ICollection<ExercisePrerequisite> PrerequisiteFor { get; set; } = new List<ExercisePrerequisite>();
    public ICollection<ExerciseReview> Reviews { get; set; } = new List<ExerciseReview>();
    public ICollection<ExerciseHint> Hints { get; set; } = new List<ExerciseHint>();
    public ICollection<UserExerciseLog> ExerciseLogs { get; set; } = new List<UserExerciseLog>();
    public ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();
    public ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = new List<TrainingPlanExercise>();
    public ICollection<LessonExercise> LessonExercises { get; set; } = new List<LessonExercise>();
}
