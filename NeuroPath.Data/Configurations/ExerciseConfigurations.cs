using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasOne(e => e.CreatedByUser).WithMany(u => u.CreatedExercises).HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.SetNull);
        builder.Property(e => e.Title).HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Instructions).HasMaxLength(2000);
        builder.Property(e => e.ImageUrl).HasMaxLength(500);
        builder.Property(e => e.VideoUrl).HasMaxLength(500);
        builder.HasIndex(e => new { e.IsActive, e.ExerciseCategoryId });
    }
}

public class ExerciseCategoryConfiguration : IEntityTypeConfiguration<ExerciseCategory>
{
    public void Configure(EntityTypeBuilder<ExerciseCategory> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100);
        builder.Property(c => c.Color).HasMaxLength(7);
        builder.HasIndex(c => c.Name).IsUnique();
    }
}

public class DifficultyLevelConfiguration : IEntityTypeConfiguration<DifficultyLevel>
{
    public void Configure(EntityTypeBuilder<DifficultyLevel> builder)
    {
        builder.Property(d => d.Name).HasMaxLength(50);
        builder.Property(d => d.Color).HasMaxLength(7);
        builder.HasIndex(d => d.Level).IsUnique();
    }
}

public class ExerciseBrainAreaConfiguration : IEntityTypeConfiguration<ExerciseBrainArea>
{
    public void Configure(EntityTypeBuilder<ExerciseBrainArea> builder)
    {
        builder.HasKey(x => new { x.ExerciseId, x.BrainAreaId });
    }
}

public class ExerciseTagConfiguration : IEntityTypeConfiguration<ExerciseTag>
{
    public void Configure(EntityTypeBuilder<ExerciseTag> builder)
    {
        builder.HasKey(x => new { x.ExerciseId, x.TagId });
    }
}

public class ExercisePrerequisiteConfiguration : IEntityTypeConfiguration<ExercisePrerequisite>
{
    public void Configure(EntityTypeBuilder<ExercisePrerequisite> builder)
    {
        builder.HasKey(x => new { x.ExerciseId, x.PrerequisiteExerciseId });
        builder.HasOne(x => x.Exercise).WithMany(e => e.Prerequisites).HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PrerequisiteExercise).WithMany(e => e.PrerequisiteFor).HasForeignKey(x => x.PrerequisiteExerciseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ExerciseReviewConfiguration : IEntityTypeConfiguration<ExerciseReview>
{
    public void Configure(EntityTypeBuilder<ExerciseReview> builder)
    {
        builder.Property(r => r.ReviewText).HasMaxLength(2000);
        builder.HasIndex(r => new { r.ExerciseId, r.UserId }).IsUnique();
    }
}

public class ExerciseHintConfiguration : IEntityTypeConfiguration<ExerciseHint>
{
    public void Configure(EntityTypeBuilder<ExerciseHint> builder)
    {
        builder.Property(h => h.HintText).HasMaxLength(1000);
        builder.HasIndex(h => new { h.ExerciseId, h.HintOrder });
    }
}

public class ExerciseMediaConfiguration : IEntityTypeConfiguration<ExerciseMedia>
{
    public void Configure(EntityTypeBuilder<ExerciseMedia> builder)
    {
        builder.Property(m => m.MediaUrl).HasMaxLength(500);
        builder.Property(m => m.Title).HasMaxLength(200);
    }
}
