using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Quizzes;

namespace NeuroPath.Data.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasOne(q => q.CreatedByUser).WithMany().HasForeignKey(q => q.CreatedByUserId);
        builder.Property(q => q.Title).HasMaxLength(200);
        builder.HasIndex(q => new { q.IsPublished, q.IsActive });
    }
}

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.Property(q => q.QuestionType).HasMaxLength(30);
        builder.Property(q => q.ImageUrl).HasMaxLength(500);
    }
}

public class UserQuizAttemptConfiguration : IEntityTypeConfiguration<UserQuizAttempt>
{
    public void Configure(EntityTypeBuilder<UserQuizAttempt> builder)
    {
        builder.HasIndex(a => new { a.UserId, a.QuizId });
        builder.HasIndex(a => new { a.UserId, a.QuizId }, "IX_UserQuizAttempts_FirstAward")
            .IsUnique()
            .HasFilter("[PointsAwarded] > 0");
    }
}

public class UserQuizAnswerConfiguration : IEntityTypeConfiguration<UserQuizAnswer>
{
    public void Configure(EntityTypeBuilder<UserQuizAnswer> builder)
    {
        builder.HasIndex(a => new { a.UserQuizAttemptId, a.QuizQuestionId }).IsUnique();
    }
}
