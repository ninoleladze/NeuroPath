using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Gamification;

namespace NeuroPath.Data.Configurations;

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.Property(a => a.Name).HasMaxLength(100);
        builder.Property(a => a.Tier).HasMaxLength(20);
        builder.Property(a => a.IconUrl).HasMaxLength(500);
        builder.HasIndex(a => a.Name).IsUnique();
    }
}

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        builder.HasKey(x => new { x.UserId, x.AchievementId });
    }
}

public class RewardConfiguration : IEntityTypeConfiguration<Reward>
{
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.Property(r => r.Name).HasMaxLength(100);
    }
}

public class UserRewardConfiguration : IEntityTypeConfiguration<UserReward>
{
    public void Configure(EntityTypeBuilder<UserReward> builder)
    {
        builder.Property(r => r.Source).HasMaxLength(100);
    }
}

public class ShopItemConfiguration : IEntityTypeConfiguration<ShopItem>
{
    public void Configure(EntityTypeBuilder<ShopItem> builder)
    {
        builder.Property(i => i.Name).HasMaxLength(100);
        builder.Property(i => i.Category).HasMaxLength(50);
        builder.Property(i => i.ImageUrl).HasMaxLength(500);
    }
}

public class StreakConfiguration : IEntityTypeConfiguration<Streak>
{
    public void Configure(EntityTypeBuilder<Streak> builder)
    {
        builder.HasOne(s => s.User).WithOne(u => u.Streak).HasForeignKey<Streak>(s => s.UserId);
        builder.HasIndex(s => s.UserId).IsUnique();
    }
}

public class LeaderboardEntryConfiguration : IEntityTypeConfiguration<LeaderboardEntry>
{
    public void Configure(EntityTypeBuilder<LeaderboardEntry> builder)
    {
        builder.Property(e => e.Period).HasMaxLength(20);
        builder.HasIndex(e => new { e.Period, e.Rank });
    }
}

public class PointTransactionConfiguration : IEntityTypeConfiguration<PointTransaction>
{
    public void Configure(EntityTypeBuilder<PointTransaction> builder)
    {
        builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
        builder.HasIndex(t => new { t.CreatedAt, t.UserId });
        builder.HasIndex(t => new { t.UserId, t.CreatedAt });
    }
}
