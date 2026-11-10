using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IFeedbackService
{
    Task<List<FeedbackDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto, string userId);
    Task<bool> RespondAsync(int id, string response);
}
