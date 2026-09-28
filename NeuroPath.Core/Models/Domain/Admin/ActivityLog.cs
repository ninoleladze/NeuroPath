using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Admin;

public class ActivityLog
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public ActivityType ActivityType { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User? User { get; set; }
}
