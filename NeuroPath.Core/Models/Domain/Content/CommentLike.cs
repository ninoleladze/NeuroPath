using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Content;

public class CommentLike
{
    public int Id { get; set; }
    public int CommentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Comment Comment { get; set; } = null!;
    public User User { get; set; } = null!;
}
