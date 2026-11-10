namespace NeuroPath.Core.Settings;

public class ClientAppSettings
{
    public string BaseUrl { get; set; } = "http://localhost:4200";
    public string[] AllowedOrigins { get; set; } = { "http://localhost:4200" };
}
