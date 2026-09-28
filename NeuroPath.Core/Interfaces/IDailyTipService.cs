namespace NeuroPath.Core.Interfaces;

public interface IDailyTipService
{
    Task<object?> GetTodayAsync();
    Task<List<object>> GetAllAsync();
}
