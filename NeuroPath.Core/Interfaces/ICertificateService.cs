using NeuroPath.Core.Models.DTOs.Assessment;

namespace NeuroPath.Core.Interfaces;

public interface ICertificateService
{
    Task<List<CertificateDto>> GetAllAsync();
    Task<List<UserCertificateDto>> GetUserCertificatesAsync(string userId);
    Task<CertificateDto> CreateAsync(CreateCertificateDto dto);
    Task<CertificateDto?> UpdateAsync(int id, CreateCertificateDto dto);
    Task<List<UserCertificateDto>> AwardForCourseAsync(string userId, int courseModuleId);
    Task<List<UserCertificateDto>> AwardForAssessmentAsync(string userId, int assessmentId, decimal scorePercent);
}
