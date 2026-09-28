using NeuroPath.Core.Enums;

namespace NeuroPath.Core.Models.DTOs.Admin;

public class ActivityLogDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public ActivityType ActivityType { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ReportDto
{
    public int Id { get; set; }
    public string ReporterUserId { get; set; } = string.Empty;
    public string ReporterUserName { get; set; } = string.Empty;
    public string ReportedEntityType { get; set; } = string.Empty;
    public int ReportedEntityId { get; set; }
    public ReportReason Reason { get; set; }
    public string? Description { get; set; }
    public ReportStatus Status { get; set; }
    public string? ReviewNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

public class CreateReportDto
{
    public string ReportedEntityType { get; set; } = string.Empty;
    public int ReportedEntityId { get; set; }
    public ReportReason Reason { get; set; }
    public string? Description { get; set; }
}

public class ReviewReportDto
{
    public ReportStatus Status { get; set; }
    public string? ReviewNotes { get; set; }
}

public class FeedbackDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsReviewed { get; set; }
    public string? AdminResponse { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateFeedbackDto
{
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public string Category { get; set; } = "General";
}

public class FaqCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<FaqDto> Faqs { get; set; } = new();
}

public class FaqDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int ViewCount { get; set; }
}

public class CreateFaqDto
{
    public int FaqCategoryId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class SupportTicketDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public string? AssignedToUserId { get; set; }
    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}

public class CreateSupportTicketDto
{
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
}

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
}

public class WeeklyChallengeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TargetExerciseCount { get; set; }
    public int TargetPoints { get; set; }
    public int BonusPoints { get; set; }
    public bool IsActive { get; set; }
    public bool IsTeamChallenge { get; set; }
    public string? ImageUrl { get; set; }
    public int ParticipantCount { get; set; }
}

public class CreateWeeklyChallengeDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TargetExerciseCount { get; set; }
    public int TargetPoints { get; set; }
    public decimal? TargetAccuracy { get; set; }
    public int BonusPoints { get; set; }
    public bool IsTeamChallenge { get; set; }
    public string? ImageUrl { get; set; }
}

public class MoodLogDto
{
    public int Id { get; set; }
    public DateTime LogDate { get; set; }
    public string MoodBefore { get; set; } = string.Empty;
    public string? MoodAfter { get; set; }
    public int EnergyLevel { get; set; }
    public int FocusLevel { get; set; }
    public int StressLevel { get; set; }
    public decimal? SleepHours { get; set; }
    public int? SleepQuality { get; set; }
    public bool PhysicalActivity { get; set; }
    public string? Notes { get; set; }
}

public class CreateMoodLogDto
{
    public MoodType MoodBefore { get; set; }
    public int EnergyLevel { get; set; }
    public int FocusLevel { get; set; }
    public int StressLevel { get; set; }
    public decimal? SleepHours { get; set; }
    public int? SleepQuality { get; set; }
    public decimal? WaterIntakeLiters { get; set; }
    public bool PhysicalActivity { get; set; }
    public string? Notes { get; set; }
}

public enum MoodType { VeryLow = 1, Low = 2, Neutral = 3, Good = 4, Excellent = 5 }
