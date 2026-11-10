namespace NeuroPath.Core.Settings;

public class FileUploadSettings
{
    public string UploadPath { get; set; } = "wwwroot/uploads";
    public string RootPath { get; set; } = string.Empty;
    public string PublicBasePath { get; set; } = "/uploads";
    public long MaxFileSizeBytes { get; set; } = 5242880;
    public long MaxAvatarSizeBytes { get; set; } = 2097152;
    public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf" };
    public string AvatarFolder { get; set; } = "avatars";
    public string ExerciseImageFolder { get; set; } = "exercises";
    public string ArticleImageFolder { get; set; } = "articles";
    public string CertificateFolder { get; set; } = "certificates";
}
