using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface IFeedbackService
{
    Task<List<FeedbackDto>> GetAllAsync();
    Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto, string userId);
    Task<bool> RespondAsync(int id, string response);
}
