using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Interfaces;

public interface IDailyTipService
{
    Task<DailyTipDto?> GetTodayAsync();
    Task<List<DailyTipAdminDto>> GetAllAsync();
    Task<DailyTipAdminDto> CreateAsync(SaveDailyTipDto dto);
    Task<DailyTipAdminDto?> UpdateAsync(int id, SaveDailyTipDto dto);
    Task<bool> DeleteAsync(int id);
}
