namespace NeuroPath.Core.Settings;

public class BackgroundJobSettings
{
    public bool Enabled { get; set; } = true;
    public int IntervalMinutes { get; set; } = 60;
    public int SnapshotIntervalDays { get; set; } = 7;
    public int ActiveUserWindowDays { get; set; } = 30;
    public int SnapshotBatchSize { get; set; } = 200;
    public int RevokedTokenRetentionDays { get; set; } = 30;
}
