namespace NeuroPath.Core.Models.DTOs.User;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? AvatarUrl { get; set; }
    public int TotalPoints { get; set; }
    public int CurrentBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class UserProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Occupation { get; set; }
    public string? CognitiveGoal { get; set; }
    public int DailyTrainingMinutes { get; set; }
}

public class UpdateProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Occupation { get; set; }
    public string? CognitiveGoal { get; set; }
    public int DailyTrainingMinutes { get; set; } = 15;
}

public class UserSettingsDto
{
    public string Language { get; set; } = "en";
    public string Theme { get; set; } = "system";
    public bool EmailNotifications { get; set; }
    public bool PushNotifications { get; set; }
    public bool StreakReminders { get; set; }
    public bool WeeklySummary { get; set; }
    public string ProfileVisibility { get; set; } = "public";
    public bool ShowOnLeaderboard { get; set; }
}

public class UpdateSettingsDto : UserSettingsDto { }

public class PublicUserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int TotalPoints { get; set; }
    public string? Country { get; set; }
}

public class SetRoleDto
{
    public string Role { get; set; } = string.Empty;
}
