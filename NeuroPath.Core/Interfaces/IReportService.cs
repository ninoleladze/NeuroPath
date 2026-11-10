using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IReportService
{
    Task<List<ReportDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ReportDto> CreateAsync(CreateReportDto dto, string userId);
    Task<bool> ReviewAsync(int id, ReviewReportDto dto, string reviewerId);
}
