namespace NeuroPath.Core.Models.DTOs.File;

public class FileResponseDto
{
    public int Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}

public sealed record FileUploadRequest(Stream Content, string FileName, string ContentType, long Length);
