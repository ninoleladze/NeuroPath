using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Assessment;

namespace NeuroPath.Data.Configurations;

public class SkillAssessmentConfiguration : IEntityTypeConfiguration<SkillAssessment>
{
    public void Configure(EntityTypeBuilder<SkillAssessment> builder)
    {
        builder.Property(a => a.Title).HasMaxLength(200);
    }
}

public class AssessmentQuestionConfiguration : IEntityTypeConfiguration<AssessmentQuestion>
{
    public void Configure(EntityTypeBuilder<AssessmentQuestion> builder)
    {
        builder.Property(q => q.QuestionType).HasMaxLength(30);
    }
}

public class UserAssessmentResultConfiguration : IEntityTypeConfiguration<UserAssessmentResult>
{
    public void Configure(EntityTypeBuilder<UserAssessmentResult> builder)
    {
        builder.HasIndex(r => new { r.UserId, r.SkillAssessmentId });
    }
}

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.Property(c => c.Title).HasMaxLength(200);
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
    }
}

public class UserCertificateConfiguration : IEntityTypeConfiguration<UserCertificate>
{
    public void Configure(EntityTypeBuilder<UserCertificate> builder)
    {
        builder.HasKey(x => new { x.UserId, x.CertificateId });
        builder.Property(x => x.CertificateNumber).HasMaxLength(50);
        builder.Property(x => x.PdfUrl).HasMaxLength(500);
        builder.HasIndex(x => x.CertificateNumber).IsUnique();
    }
}
