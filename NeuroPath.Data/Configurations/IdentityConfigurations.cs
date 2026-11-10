using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.AvatarUrl).HasMaxLength(500);
        builder.HasIndex(u => u.TotalPoints);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Description).HasMaxLength(200);
    }
}

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasOne(p => p.User).WithOne(u => u.UserProfile).HasForeignKey<UserProfile>(p => p.UserId);
        builder.HasIndex(p => p.UserId).IsUnique();
        builder.Property(p => p.FirstName).HasMaxLength(50);
        builder.Property(p => p.LastName).HasMaxLength(50);
        builder.Property(p => p.Bio).HasMaxLength(1000);
        builder.Property(p => p.Country).HasMaxLength(100);
        builder.Property(p => p.City).HasMaxLength(100);
        builder.Property(p => p.Occupation).HasMaxLength(100);
        builder.Property(p => p.CognitiveGoal).HasMaxLength(500);
    }
}

public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.HasOne(s => s.User).WithOne(u => u.UserSettings).HasForeignKey<UserSettings>(s => s.UserId);
        builder.HasIndex(s => s.UserId).IsUnique();
        builder.Property(s => s.Language).HasMaxLength(10);
        builder.Property(s => s.Theme).HasMaxLength(20);
        builder.Property(s => s.ProfileVisibility).HasMaxLength(20);
    }
}

public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.Property(t => t.Token).HasMaxLength(128);
        builder.HasIndex(t => t.Token).IsUnique();
    }
}

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.Property(t => t.Token).HasMaxLength(128);
        builder.Property(t => t.IpAddress).HasMaxLength(45);
        builder.HasIndex(t => t.Token).IsUnique();
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasOne(t => t.User).WithMany(u => u.RefreshTokens).HasForeignKey(t => t.UserId);
        builder.Property(t => t.TokenHash).HasMaxLength(128);
        builder.Property(t => t.ReplacedByTokenHash).HasMaxLength(128);
        builder.Property(t => t.CreatedByIp).HasMaxLength(45);
        builder.HasIndex(t => t.TokenHash).IsUnique();
        builder.HasIndex(t => new { t.UserId, t.RevokedAt });
    }
}
