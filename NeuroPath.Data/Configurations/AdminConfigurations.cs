using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.Domain.FileManagement;

namespace NeuroPath.Data.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.HasOne(l => l.User).WithMany(u => u.ActivityLogs).HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.SetNull);
        builder.Property(l => l.Description).HasMaxLength(1000);
        builder.Property(l => l.EntityType).HasMaxLength(50);
        builder.Property(l => l.IpAddress).HasMaxLength(45);
        builder.HasIndex(l => l.CreatedAt);
    }
}

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasOne(r => r.Reporter).WithMany(u => u.Reports).HasForeignKey(r => r.ReporterUserId);
        builder.HasOne(r => r.ReviewedBy).WithMany().HasForeignKey(r => r.ReviewedByUserId).OnDelete(DeleteBehavior.SetNull);
        builder.Property(r => r.ReportedEntityType).HasMaxLength(50);
        builder.Property(r => r.Description).HasMaxLength(2000);
        builder.Property(r => r.ReviewNotes).HasMaxLength(2000);
        builder.HasIndex(r => new { r.Status, r.CreatedAt });
    }
}

public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.HasOne(t => t.User).WithMany(u => u.SupportTickets).HasForeignKey(t => t.UserId);
        builder.HasOne(t => t.AssignedTo).WithMany().HasForeignKey(t => t.AssignedToUserId).OnDelete(DeleteBehavior.SetNull);
        builder.Property(t => t.Subject).HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(4000);
        builder.Property(t => t.Resolution).HasMaxLength(4000);
        builder.HasIndex(t => new { t.Status, t.CreatedAt });
    }
}

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.Property(f => f.Subject).HasMaxLength(200);
        builder.Property(f => f.Message).HasMaxLength(4000);
        builder.Property(f => f.Category).HasMaxLength(50);
        builder.Property(f => f.AdminResponse).HasMaxLength(4000);
    }
}

public class FaqCategoryConfiguration : IEntityTypeConfiguration<FaqCategory>
{
    public void Configure(EntityTypeBuilder<FaqCategory> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100);
    }
}

public class FaqConfiguration : IEntityTypeConfiguration<Faq>
{
    public void Configure(EntityTypeBuilder<Faq> builder)
    {
        builder.Property(f => f.Question).HasMaxLength(500);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(n => n.Title).HasMaxLength(200);
        builder.Property(n => n.Message).HasMaxLength(1000);
        builder.Property(n => n.ReferenceType).HasMaxLength(50);
        builder.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt });
    }
}

public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
{
    public void Configure(EntityTypeBuilder<UploadedFile> builder)
    {
        builder.Property(f => f.FileName).HasMaxLength(255);
        builder.Property(f => f.StoredFileName).HasMaxLength(100);
        builder.Property(f => f.FilePath).HasMaxLength(500);
        builder.Property(f => f.FileUrl).HasMaxLength(500);
        builder.Property(f => f.ContentType).HasMaxLength(100);
    }
}
