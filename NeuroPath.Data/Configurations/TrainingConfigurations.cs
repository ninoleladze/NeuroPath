using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Training;

namespace NeuroPath.Data.Configurations;

public class TrainingPlanConfiguration : IEntityTypeConfiguration<TrainingPlan>
{
    public void Configure(EntityTypeBuilder<TrainingPlan> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(1000);
    }
}

public class TrainingPlanExerciseConfiguration : IEntityTypeConfiguration<TrainingPlanExercise>
{
    public void Configure(EntityTypeBuilder<TrainingPlanExercise> builder)
    {
        builder.HasIndex(x => new { x.TrainingPlanId, x.DayOfWeek, x.OrderInDay });
    }
}

public class DailySessionConfiguration : IEntityTypeConfiguration<DailySession>
{
    public void Configure(EntityTypeBuilder<DailySession> builder)
    {
        builder.HasOne(s => s.TrainingPlan).WithMany(p => p.Sessions).HasForeignKey(s => s.TrainingPlanId).OnDelete(DeleteBehavior.SetNull);
        builder.Property(s => s.Notes).HasMaxLength(1000);
        builder.HasIndex(s => new { s.UserId, s.SessionDate });
    }
}

public class UserExerciseLogConfiguration : IEntityTypeConfiguration<UserExerciseLog>
{
    public void Configure(EntityTypeBuilder<UserExerciseLog> builder)
    {
        builder.HasIndex(l => new { l.UserId, l.CompletedAt });
    }
}
