using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class ReportService : IReportService
{
    public static readonly string[] ReportableTypes = { "Comment", "Article", "Exercise", "Team", "User" };

    private readonly INeuroPathDbContext _db;

    public ReportService(INeuroPathDbContext db) => _db = db;

    public async Task<List<ReportDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.Reports.AsNoTracking()
            .OrderBy(r => r.Status).ThenByDescending(r => r.CreatedAt)
            .Skip(skip).Take(take)
            .Select(r => new ReportDto
            {
                Id = r.Id,
                ReporterUserId = r.ReporterUserId,
                ReporterUserName = r.Reporter.UserName!,
                ReportedEntityType = r.ReportedEntityType,
                ReportedEntityId = r.ReportedEntityId,
                Reason = r.Reason,
                Description = r.Description,
                Status = r.Status,
                ReviewNotes = r.ReviewNotes,
                CreatedAt = r.CreatedAt,
                ReviewedAt = r.ReviewedAt
            })
            .ToListAsync();
    }

    public async Task<ReportDto> CreateAsync(CreateReportDto dto, string userId)
    {
        var type = ReportableTypes.FirstOrDefault(t => string.Equals(t, dto.ReportedEntityType, StringComparison.OrdinalIgnoreCase))
            ?? throw new BusinessRuleException($"Reported entity type must be one of: {string.Join(", ", ReportableTypes)}.");

        if (!await TargetExistsAsync(type, dto.ReportedEntityId))
            throw new NotFoundException($"{type} {dto.ReportedEntityId} was not found.");

        if (await _db.Reports.AnyAsync(r => r.ReporterUserId == userId && r.ReportedEntityType == type
            && r.ReportedEntityId == dto.ReportedEntityId && r.Status == ReportStatus.Pending))
            throw new ConflictException("You have already reported this item.");

        var report = new Report
        {
            ReporterUserId = userId,
            ReportedEntityType = type,
            ReportedEntityId = dto.ReportedEntityId,
            Reason = dto.Reason,
            Description = dto.Description
        };
        _db.Reports.Add(report);
        await _db.SaveChangesAsync();

        var userName = await _db.Users.Where(u => u.Id == userId).Select(u => u.UserName).FirstOrDefaultAsync();
        return new ReportDto
        {
            Id = report.Id,
            ReporterUserId = userId,
            ReporterUserName = userName ?? string.Empty,
            ReportedEntityType = report.ReportedEntityType,
            ReportedEntityId = report.ReportedEntityId,
            Reason = report.Reason,
            Description = report.Description,
            Status = report.Status,
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<bool> ReviewAsync(int id, ReviewReportDto dto, string reviewerId)
        => await _db.Reports.Where(r => r.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(r => r.Status, dto.Status)
                .SetProperty(r => r.ReviewNotes, dto.ReviewNotes)
                .SetProperty(r => r.ReviewedByUserId, reviewerId)
                .SetProperty(r => r.ReviewedAt, DateTime.UtcNow)) > 0;

    private Task<bool> TargetExistsAsync(string type, int id) => type switch
    {
        "Comment" => _db.Comments.AnyAsync(c => c.Id == id),
        "Article" => _db.Articles.AnyAsync(a => a.Id == id),
        "Exercise" => _db.Exercises.AnyAsync(e => e.Id == id),
        "Team" => _db.Teams.AnyAsync(t => t.Id == id),
        "User" => Task.FromResult(true),
        _ => Task.FromResult(false)
    };
}
