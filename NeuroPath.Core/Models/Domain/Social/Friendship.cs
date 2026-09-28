using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Social;

public class Friendship
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FriendId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public User Friend { get; set; } = null!;
}
