namespace NeuroPath.Core.Settings;
public class RateLimitSettings
{
    public int PermitLimit { get; set; } = 100;
    public int WindowSeconds { get; set; } = 60;
    public int QueueLimit { get; set; } = 2;
    public int AuthEndpointPermitLimit { get; set; } = 10;
    public int AuthEndpointWindowSeconds { get; set; } = 300;
    public int FileUploadPermitLimit { get; set; } = 20;
    public int FileUploadWindowSeconds { get; set; } = 300;
}
