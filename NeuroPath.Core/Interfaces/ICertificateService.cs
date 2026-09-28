using NeuroPath.Core.Models.DTOs.Assessment;

namespace NeuroPath.Core.Interfaces;

public interface ICertificateService
{
    Task<List<CertificateDto>> GetAllAsync();
    Task<List<UserCertificateDto>> GetUserCertificatesAsync(string userId);
    Task<CertificateDto> CreateAsync(CertificateDto dto);
    Task<CertificateDto?> UpdateAsync(int id, CertificateDto dto);
}
