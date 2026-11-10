using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.Domain.Wellness;

namespace NeuroPath.Data.Configurations;

public class BrainAreaConfiguration : IEntityTypeConfiguration<BrainArea>
{
    public void Configure(EntityTypeBuilder<BrainArea> builder)
    {
        builder.Property(b => b.Name).HasMaxLength(100);
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.Property(b => b.Color).HasMaxLength(7);
        builder.Property(b => b.IconUrl).HasMaxLength(500);
        builder.HasIndex(b => b.Name).IsUnique();
    }
}

public class BrainRegionConfiguration : IEntityTypeConfiguration<BrainRegion>
{
    public void Configure(EntityTypeBuilder<BrainRegion> builder)
    {
        builder.Property(r => r.Name).HasMaxLength(100);
        builder.Property(r => r.LatinName).HasMaxLength(100);
        builder.Property(r => r.ImageUrl).HasMaxLength(500);
    }
}

public class BrainRegionBrainAreaConfiguration : IEntityTypeConfiguration<BrainRegionBrainArea>
{
    public void Configure(EntityTypeBuilder<BrainRegionBrainArea> builder)
    {
        builder.HasKey(x => new { x.BrainRegionId, x.BrainAreaId });
    }
}

public class NeurotransmitterConfiguration : IEntityTypeConfiguration<Neurotransmitter>
{
    public void Configure(EntityTypeBuilder<Neurotransmitter> builder)
    {
        builder.Property(n => n.Name).HasMaxLength(100);
        builder.Property(n => n.ChemicalFormula).HasMaxLength(50);
        builder.Property(n => n.ImageUrl).HasMaxLength(500);
    }
}

public class BrainAreaNeurotransmitterConfiguration : IEntityTypeConfiguration<BrainAreaNeurotransmitter>
{
    public void Configure(EntityTypeBuilder<BrainAreaNeurotransmitter> builder)
    {
        builder.HasKey(x => new { x.BrainAreaId, x.NeurotransmitterId });
    }
}

public class CognitiveSkillConfiguration : IEntityTypeConfiguration<CognitiveSkill>
{
    public void Configure(EntityTypeBuilder<CognitiveSkill> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100);
        builder.Property(c => c.MeasurementUnit).HasMaxLength(20);
    }
}

public class CognitiveSkillBrainAreaConfiguration : IEntityTypeConfiguration<CognitiveSkillBrainArea>
{
    public void Configure(EntityTypeBuilder<CognitiveSkillBrainArea> builder)
    {
        builder.HasKey(x => new { x.CognitiveSkillId, x.BrainAreaId });
    }
}

public class UserBrainAreaConfiguration : IEntityTypeConfiguration<UserBrainArea>
{
    public void Configure(EntityTypeBuilder<UserBrainArea> builder)
    {
        builder.HasKey(x => new { x.UserId, x.BrainAreaId });
    }
}

public class MoodLogConfiguration : IEntityTypeConfiguration<MoodLog>
{
    public void Configure(EntityTypeBuilder<MoodLog> builder)
    {
        builder.Property(m => m.Notes).HasMaxLength(1000);
        builder.HasIndex(m => new { m.UserId, m.LogDate });
    }
}
