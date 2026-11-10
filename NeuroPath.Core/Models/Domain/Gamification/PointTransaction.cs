using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class PointTransaction
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int Amount { get; set; }
    public PointSource Source { get; set; }
    public int? ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
}
