namespace NeuroPath.Core.Models.Domain.Identity;

public class UserSettings
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Language { get; set; } = "en";
    public string Theme { get; set; } = "system";
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = true;
    public bool StreakReminders { get; set; } = true;
    public bool WeeklySummary { get; set; } = true;
    public string ProfileVisibility { get; set; } = "public";
    public bool ShowOnLeaderboard { get; set; } = true;
    public string TimeZoneId { get; set; } = "UTC";

    public User User { get; set; } = null!;
}
