using NeuroPath.Core.Enums;

namespace NeuroPath.Core.Models.DTOs.Social;

public class FriendRequestDto
{
    public int Id { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string SenderUserName { get; set; } = string.Empty;
    public string? SenderAvatarUrl { get; set; }
    public FriendRequestStatus Status { get; set; }
    public DateTime SentAt { get; set; }
    public string? Message { get; set; }
}

public class SendFriendRequestDto
{
    public string ReceiverId { get; set; } = string.Empty;
    public string? Message { get; set; }
}

public class FriendshipDto
{
    public string FriendId { get; set; } = string.Empty;
    public string FriendUserName { get; set; } = string.Empty;
    public string? FriendAvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DirectMessageDto
{
    public int Id { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string SenderUserName { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}

public class SendMessageDto
{
    public string ReceiverId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
    public int MaxMembers { get; set; }
    public int TotalPoints { get; set; }
    public bool IsActive { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTeamDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int MaxMembers { get; set; } = 10;
}

public class TeamMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public TeamRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public int ContributedPoints { get; set; }
}
