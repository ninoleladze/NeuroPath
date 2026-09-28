using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Assessment;

public class UserCertificate
{
    public string UserId { get; set; } = string.Empty;
    public int CertificateId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    public string CertificateNumber { get; set; } = string.Empty;
    public string? PdfUrl { get; set; }

    public User User { get; set; } = null!;
    public Certificate Certificate { get; set; } = null!;
}
