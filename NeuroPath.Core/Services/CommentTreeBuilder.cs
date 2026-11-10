using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Services;

internal static class CommentTreeBuilder
{
    public static List<CommentDto> Build(List<CommentDto> flat)
    {
        var byId = flat.ToDictionary(c => c.Id);
        var roots = new List<CommentDto>();
        foreach (var comment in flat)
        {
            if (comment.ParentCommentId.HasValue && byId.TryGetValue(comment.ParentCommentId.Value, out var parent))
                parent.Replies.Add(comment);
            else if (!comment.ParentCommentId.HasValue)
                roots.Add(comment);
        }
        return roots;
    }
}
