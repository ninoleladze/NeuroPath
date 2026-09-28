using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Admin;

public class Report
{
    public int Id { get; set; }
    public string ReporterUserId { get; set; } = string.Empty;
    public string ReportedEntityType { get; set; } = string.Empty;
    public int ReportedEntityId { get; set; }
    public ReportReason Reason { get; set; }
    public string? Description { get; set; }
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    public string? ReviewedByUserId { get; set; }
    public string? ReviewNotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
    public User Reporter { get; set; } = null!;
    public User? ReviewedBy { get; set; }
}
