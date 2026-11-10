namespace NeuroPath.Core.Settings;

public class GamificationSettings
{
    public int MaxScoredRepeatsPerExercisePerDay { get; set; } = 3;
    public int DailyExercisePointCap { get; set; } = 2000;
    public decimal MinimumDurationFraction { get; set; } = 0.25m;
}
