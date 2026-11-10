using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Courses;

namespace NeuroPath.Data.Configurations;

public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.HasOne(c => c.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedByUserId);
        builder.Property(c => c.Title).HasMaxLength(200);
        builder.Property(c => c.ShortDescription).HasMaxLength(500);
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
    }
}

public class ModuleLessonConfiguration : IEntityTypeConfiguration<ModuleLesson>
{
    public void Configure(EntityTypeBuilder<ModuleLesson> builder)
    {
        builder.Property(l => l.Title).HasMaxLength(200);
        builder.Property(l => l.VideoUrl).HasMaxLength(500);
        builder.HasIndex(l => new { l.CourseModuleId, l.OrderIndex });
    }
}

public class LessonExerciseConfiguration : IEntityTypeConfiguration<LessonExercise>
{
    public void Configure(EntityTypeBuilder<LessonExercise> builder)
    {
        builder.HasIndex(x => new { x.ModuleLessonId, x.ExerciseId }).IsUnique();
    }
}

public class CourseModuleTagConfiguration : IEntityTypeConfiguration<CourseModuleTag>
{
    public void Configure(EntityTypeBuilder<CourseModuleTag> builder)
    {
        builder.HasKey(x => new { x.CourseModuleId, x.TagId });
    }
}

public class UserModuleProgressConfiguration : IEntityTypeConfiguration<UserModuleProgress>
{
    public void Configure(EntityTypeBuilder<UserModuleProgress> builder)
    {
        builder.HasIndex(p => new { p.UserId, p.CourseModuleId }).IsUnique();
    }
}

public class UserLessonCompletionConfiguration : IEntityTypeConfiguration<UserLessonCompletion>
{
    public void Configure(EntityTypeBuilder<UserLessonCompletion> builder)
    {
        builder.HasKey(x => new { x.UserId, x.ModuleLessonId });
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.ModuleLesson).WithMany().HasForeignKey(x => x.ModuleLessonId);
    }
}
