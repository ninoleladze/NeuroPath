using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Communication;

public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public int? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
    public User User { get; set; } = null!;
}
