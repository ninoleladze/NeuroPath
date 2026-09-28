using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Interfaces;

public interface ISupportTicketService
{
    Task<List<SupportTicketDto>> GetAllAsync();
    Task<List<SupportTicketDto>> GetUserTicketsAsync(string userId);
    Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto, string userId);
    Task<bool> ResolveAsync(int id, string resolution);
}
