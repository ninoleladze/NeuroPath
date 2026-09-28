using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Content;

public class Bookmark
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public BookmarkType BookmarkType { get; set; }
    public int ReferenceId { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
}
