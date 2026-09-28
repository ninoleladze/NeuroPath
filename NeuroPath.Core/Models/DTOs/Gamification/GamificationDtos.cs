using NeuroPath.Core.Enums;

namespace NeuroPath.Core.Models.DTOs.Gamification;

public class AchievementDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string Tier { get; set; } = "Bronze";
    public int BonusPoints { get; set; }
    public bool IsSecret { get; set; }
    public int? PointsRequired { get; set; }
    public int? ExercisesRequired { get; set; }
    public int? StreakDaysRequired { get; set; }
}

public class UserAchievementDto
{
    public int AchievementId { get; set; }
    public string AchievementName { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public DateTime EarnedAt { get; set; }
}

public class RewardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RewardType RewardType { get; set; }
    public int PointsValue { get; set; }
    public string? IconUrl { get; set; }
}

public class ShopItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public int? StockQuantity { get; set; }
    public bool IsActive { get; set; }
}

public class UserInventoryDto
{
    public int Id { get; set; }
    public int ShopItemId { get; set; }
    public string ShopItemName { get; set; } = string.Empty;
    public DateTime PurchasedAt { get; set; }
    public bool IsEquipped { get; set; }
}

public class StreakDto
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime LastActivityDate { get; set; }
    public int TotalActiveDays { get; set; }
    public int StreakFreezeCount { get; set; }
}

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int TotalPoints { get; set; }
    public int ExercisesCompleted { get; set; }
    public int QuizzesCompleted { get; set; }
}

public class ProgressSnapshotDto
{
    public int Id { get; set; }
    public DateTime SnapshotDate { get; set; }
    public int TotalPoints { get; set; }
    public int ExercisesCompleted { get; set; }
    public int QuizzesCompleted { get; set; }
    public int CourseLessonsCompleted { get; set; }
    public decimal AverageAccuracy { get; set; }
    public int TotalTrainingMinutes { get; set; }
    public int StreakDays { get; set; }
    public string? TopBrainArea { get; set; }
    public string? WeakestBrainArea { get; set; }
}
