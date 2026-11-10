using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Assessment;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.DTOs.Assessment;

namespace NeuroPath.Core.Services;

public class CertificateService : ICertificateService
{
    private readonly INeuroPathDbContext _db;

    public CertificateService(INeuroPathDbContext db) => _db = db;

    public async Task<List<CertificateDto>> GetAllAsync()
        => await _db.Certificates.AsNoTracking()
            .OrderBy(c => c.Title)
            .Select(c => new CertificateDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CertificateType = c.CertificateType,
                ImageUrl = c.ImageUrl,
                RequiredCourseModuleId = c.RequiredCourseModuleId,
                RequiredAssessmentId = c.RequiredAssessmentId,
                RequiredScorePercent = c.RequiredScorePercent
            })
            .ToListAsync();

    public async Task<List<UserCertificateDto>> GetUserCertificatesAsync(string userId)
        => await _db.UserCertificates.AsNoTracking()
            .Where(uc => uc.UserId == userId)
            .OrderByDescending(uc => uc.EarnedAt)
            .Select(uc => new UserCertificateDto
            {
                CertificateId = uc.CertificateId,
                CertificateTitle = uc.Certificate.Title,
                EarnedAt = uc.EarnedAt,
                CertificateNumber = uc.CertificateNumber,
                PdfUrl = uc.PdfUrl
            })
            .ToListAsync();

    public async Task<CertificateDto> CreateAsync(CreateCertificateDto dto)
    {
        await ValidateRequirementsAsync(dto);
        var certificate = new Certificate();
        Apply(certificate, dto);
        _db.Certificates.Add(certificate);
        await _db.SaveChangesAsync();
        return ToDto(certificate);
    }

    public async Task<CertificateDto?> UpdateAsync(int id, CreateCertificateDto dto)
    {
        var certificate = await _db.Certificates.FindAsync(id);
        if (certificate == null) return null;
        await ValidateRequirementsAsync(dto);
        Apply(certificate, dto);
        await _db.SaveChangesAsync();
        return ToDto(certificate);
    }

    public async Task<List<UserCertificateDto>> AwardForCourseAsync(string userId, int courseModuleId)
    {
        var eligible = await _db.Certificates
            .Where(c => c.RequiredCourseModuleId == courseModuleId && !c.UserCertificates.Any(uc => uc.UserId == userId))
            .ToListAsync();
        return await AwardAsync(userId, eligible);
    }

    public async Task<List<UserCertificateDto>> AwardForAssessmentAsync(string userId, int assessmentId, decimal scorePercent)
    {
        var eligible = await _db.Certificates
            .Where(c => c.RequiredAssessmentId == assessmentId
                && (c.RequiredScorePercent == null || scorePercent >= c.RequiredScorePercent)
                && !c.UserCertificates.Any(uc => uc.UserId == userId))
            .ToListAsync();
        return await AwardAsync(userId, eligible);
    }

    private async Task<List<UserCertificateDto>> AwardAsync(string userId, List<Certificate> certificates)
    {
        var awarded = new List<UserCertificateDto>();
        if (certificates.Count == 0) return awarded;

        foreach (var certificate in certificates)
        {
            var userCertificate = new UserCertificate
            {
                UserId = userId,
                CertificateId = certificate.Id,
                CertificateNumber = $"NP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..10].ToUpperInvariant()}"
            };
            _db.UserCertificates.Add(userCertificate);
            _db.Notifications.Add(new Notification
            {
                UserId = userId,
                Title = "Certificate earned",
                Message = $"Congratulations! You earned the \"{certificate.Title}\" certificate.",
                Type = NotificationType.CertificateEarned,
                ReferenceId = certificate.Id,
                ReferenceType = nameof(Certificate)
            });
            awarded.Add(new UserCertificateDto
            {
                CertificateId = certificate.Id,
                CertificateTitle = certificate.Title,
                EarnedAt = userCertificate.EarnedAt,
                CertificateNumber = userCertificate.CertificateNumber
            });
        }

        await _db.SaveChangesAsync();
        return awarded;
    }

    private async Task ValidateRequirementsAsync(CreateCertificateDto dto)
    {
        if (dto.RequiredCourseModuleId.HasValue && !await _db.CourseModules.AnyAsync(c => c.Id == dto.RequiredCourseModuleId))
            throw new BusinessRuleException($"Course module {dto.RequiredCourseModuleId} does not exist.");
        if (dto.RequiredAssessmentId.HasValue && !await _db.SkillAssessments.AnyAsync(a => a.Id == dto.RequiredAssessmentId))
            throw new BusinessRuleException($"Skill assessment {dto.RequiredAssessmentId} does not exist.");
    }

    private static void Apply(Certificate c, CreateCertificateDto dto)
    {
        c.Title = dto.Title;
        c.Description = dto.Description;
        c.CertificateType = dto.CertificateType;
        c.ImageUrl = dto.ImageUrl;
        c.RequiredCourseModuleId = dto.RequiredCourseModuleId;
        c.RequiredAssessmentId = dto.RequiredAssessmentId;
        c.RequiredScorePercent = dto.RequiredScorePercent;
    }

    private static CertificateDto ToDto(Certificate c) => new()
    {
        Id = c.Id,
        Title = c.Title,
        Description = c.Description,
        CertificateType = c.CertificateType,
        ImageUrl = c.ImageUrl,
        RequiredCourseModuleId = c.RequiredCourseModuleId,
        RequiredAssessmentId = c.RequiredAssessmentId,
        RequiredScorePercent = c.RequiredScorePercent
    };
}
