using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Assessment;
using NeuroPath.Core.Models.DTOs.Assessment;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class CertificateService : ICertificateService
{
    private readonly NeuroPathDbContext _db;

    public CertificateService(NeuroPathDbContext db) => _db = db;

    public async Task<List<CertificateDto>> GetAllAsync()
        => await _db.Certificates
            .Select(c => new CertificateDto { Id = c.Id, Title = c.Title, Description = c.Description, CertificateType = c.CertificateType, ImageUrl = c.ImageUrl, RequiredScorePercent = c.RequiredScorePercent })
            .ToListAsync();

    public async Task<List<UserCertificateDto>> GetUserCertificatesAsync(string userId)
        => await _db.UserCertificates.Where(uc => uc.UserId == userId)
            .Include(uc => uc.Certificate)
            .Select(uc => new UserCertificateDto { CertificateId = uc.CertificateId, CertificateTitle = uc.Certificate.Title, EarnedAt = uc.EarnedAt, CertificateNumber = uc.CertificateNumber, PdfUrl = uc.PdfUrl })
            .ToListAsync();

    public async Task<CertificateDto> CreateAsync(CertificateDto dto)
    {
        var c = new Certificate { Title = dto.Title, Description = dto.Description, CertificateType = dto.CertificateType, ImageUrl = dto.ImageUrl, RequiredScorePercent = dto.RequiredScorePercent };
        _db.Certificates.Add(c);
        await _db.SaveChangesAsync();
        return new CertificateDto { Id = c.Id, Title = c.Title, Description = c.Description, CertificateType = c.CertificateType, ImageUrl = c.ImageUrl, RequiredScorePercent = c.RequiredScorePercent };
    }

    public async Task<CertificateDto?> UpdateAsync(int id, CertificateDto dto)
    {
        var c = await _db.Certificates.FindAsync(id);
        if (c == null) return null;
        c.Title = dto.Title; c.Description = dto.Description; c.CertificateType = dto.CertificateType;
        c.ImageUrl = dto.ImageUrl; c.RequiredScorePercent = dto.RequiredScorePercent;
        await _db.SaveChangesAsync();
        return new CertificateDto { Id = c.Id, Title = c.Title, Description = c.Description, CertificateType = c.CertificateType, ImageUrl = c.ImageUrl, RequiredScorePercent = c.RequiredScorePercent };
    }
}
