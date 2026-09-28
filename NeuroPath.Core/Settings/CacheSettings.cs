namespace NeuroPath.Core.Settings;
public class CacheSettings
{
    public int DefaultExpirationMinutes { get; set; } = 10;
    public int BrainAreasCacheMinutes { get; set; } = 60;
    public int ExerciseCategoriesCacheMinutes { get; set; } = 60;
    public int DifficultyLevelsCacheMinutes { get; set; } = 120;
    public int TagsCacheMinutes { get; set; } = 60;
    public int FaqCacheMinutes { get; set; } = 30;
    public int LeaderboardCacheMinutes { get; set; } = 5;
    public int DailyTipCacheMinutes { get; set; } = 1440;
}
