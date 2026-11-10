using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface ISupportTicketService
{
    Task<List<SupportTicketDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<List<SupportTicketDto>> GetUserTicketsAsync(string userId);
    Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto, string userId);
    Task<bool> AssignAsync(int id, string assigneeUserId);
    Task<bool> ResolveAsync(int id, string resolution, string resolverUserId);
}
