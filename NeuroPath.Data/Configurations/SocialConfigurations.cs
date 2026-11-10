using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Challenges;
using NeuroPath.Core.Models.Domain.Social;

namespace NeuroPath.Data.Configurations;

public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.HasOne(f => f.Sender).WithMany(u => u.SentFriendRequests).HasForeignKey(f => f.SenderId);
        builder.HasOne(f => f.Receiver).WithMany(u => u.ReceivedFriendRequests).HasForeignKey(f => f.ReceiverId);
        builder.Property(f => f.Message).HasMaxLength(500);
        builder.HasIndex(f => new { f.SenderId, f.ReceiverId }).IsUnique().HasFilter("[Status] = 0");
        builder.HasIndex(f => new { f.ReceiverId, f.Status });
    }
}

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasOne(f => f.User).WithMany(u => u.FriendshipsInitiated).HasForeignKey(f => f.UserId);
        builder.HasOne(f => f.Friend).WithMany(u => u.FriendshipsReceived).HasForeignKey(f => f.FriendId);
        builder.HasIndex(f => new { f.UserId, f.FriendId }).IsUnique();
    }
}

public class DirectMessageConfiguration : IEntityTypeConfiguration<DirectMessage>
{
    public void Configure(EntityTypeBuilder<DirectMessage> builder)
    {
        builder.HasOne(m => m.Sender).WithMany(u => u.SentMessages).HasForeignKey(m => m.SenderId);
        builder.HasOne(m => m.Receiver).WithMany(u => u.ReceivedMessages).HasForeignKey(m => m.ReceiverId);
        builder.Property(m => m.Content).HasMaxLength(2000);
        builder.HasIndex(m => new { m.SenderId, m.ReceiverId, m.SentAt });
        builder.HasIndex(m => new { m.ReceiverId, m.IsRead });
    }
}

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasOne(t => t.CreatedByUser).WithMany().HasForeignKey(t => t.CreatedByUserId);
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.AvatarUrl).HasMaxLength(500);
        builder.HasIndex(t => t.Name).IsUnique();
    }
}

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.HasKey(x => new { x.UserId, x.TeamId });
        builder.HasIndex(x => x.TeamId);
    }
}

public class WeeklyChallengeConfiguration : IEntityTypeConfiguration<WeeklyChallenge>
{
    public void Configure(EntityTypeBuilder<WeeklyChallenge> builder)
    {
        builder.Property(c => c.Title).HasMaxLength(200);
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
        builder.HasIndex(c => new { c.IsActive, c.StartDate, c.EndDate });
    }
}

public class UserChallengeConfiguration : IEntityTypeConfiguration<UserChallenge>
{
    public void Configure(EntityTypeBuilder<UserChallenge> builder)
    {
        builder.HasKey(x => new { x.UserId, x.WeeklyChallengeId });
        builder.HasIndex(x => new { x.WeeklyChallengeId, x.EarnedPoints });
    }
}

public class TeamChallengeEntryConfiguration : IEntityTypeConfiguration<TeamChallengeEntry>
{
    public void Configure(EntityTypeBuilder<TeamChallengeEntry> builder)
    {
        builder.HasKey(x => new { x.TeamId, x.WeeklyChallengeId });
    }
}

public class ChallengeRewardConfiguration : IEntityTypeConfiguration<ChallengeReward>
{
    public void Configure(EntityTypeBuilder<ChallengeReward> builder)
    {
        builder.HasIndex(x => new { x.WeeklyChallengeId, x.RewardId, x.PlaceRequired }).IsUnique();
    }
}
