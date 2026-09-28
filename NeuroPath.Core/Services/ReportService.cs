using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class ReportService : IReportService
{
    private readonly NeuroPathDbContext _db;

    public ReportService(NeuroPathDbContext db) => _db = db;

    public async Task<List<ReportDto>> GetAllAsync()
        => await _db.Reports.Include(r => r.Reporter).Include(r => r.ReviewedBy)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReportDto { Id = r.Id, ReporterUserId = r.ReporterUserId, ReporterUserName = r.Reporter.UserName!, ReportedEntityType = r.ReportedEntityType, ReportedEntityId = r.ReportedEntityId, Reason = r.Reason, Description = r.Description, Status = r.Status, ReviewNotes = r.ReviewNotes, CreatedAt = r.CreatedAt, ReviewedAt = r.ReviewedAt })
            .ToListAsync();

    public async Task<ReportDto> CreateAsync(CreateReportDto dto, string userId)
    {
        var r = new Report { ReporterUserId = userId, ReportedEntityType = dto.ReportedEntityType, ReportedEntityId = dto.ReportedEntityId, Reason = dto.Reason, Description = dto.Description };
        _db.Reports.Add(r);
        await _db.SaveChangesAsync();
        var user = await _db.Users.FindAsync(userId);
        return new ReportDto { Id = r.Id, ReporterUserId = r.ReporterUserId, ReporterUserName = user?.UserName ?? userId, ReportedEntityType = r.ReportedEntityType, ReportedEntityId = r.ReportedEntityId, Reason = r.Reason, Status = r.Status, CreatedAt = r.CreatedAt };
    }

    public async Task<bool> ReviewAsync(int id, ReviewReportDto dto, string reviewerId)
    {
        var r = await _db.Reports.FindAsync(id);
        if (r == null) return false;
        r.Status = dto.Status; r.ReviewNotes = dto.ReviewNotes; r.ReviewedByUserId = reviewerId; r.ReviewedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }
}
