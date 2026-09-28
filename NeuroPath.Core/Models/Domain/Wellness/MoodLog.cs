using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Wellness;

public class MoodLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime LogDate { get; set; } = DateTime.UtcNow;
    public MoodType MoodBefore { get; set; }
    public MoodType? MoodAfter { get; set; }
    public int EnergyLevel { get; set; }
    public int FocusLevel { get; set; }
    public int StressLevel { get; set; }
    public decimal? SleepHours { get; set; }
    public int? SleepQuality { get; set; }
    public decimal? WaterIntakeLiters { get; set; }
    public bool PhysicalActivity { get; set; }
    public string? Notes { get; set; }
    public User User { get; set; } = null!;
}
