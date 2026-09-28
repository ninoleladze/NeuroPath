namespace NeuroPath.Core.Models.Domain.Gamification;

public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int? PointsRequired { get; set; }
    public int? ExercisesRequired { get; set; }
    public int? StreakDaysRequired { get; set; }
    public int? QuizzesRequired { get; set; }
    public int? CoursesRequired { get; set; }
    public string Tier { get; set; } = "Bronze";
    public int BonusPoints { get; set; }
    public bool IsSecret { get; set; } = false;

    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
