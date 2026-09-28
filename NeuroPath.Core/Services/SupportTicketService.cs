using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class SupportTicketService : ISupportTicketService
{
    private readonly NeuroPathDbContext _db;

    public SupportTicketService(NeuroPathDbContext db) => _db = db;

    public async Task<List<SupportTicketDto>> GetAllAsync()
        => await _db.SupportTickets.Include(t => t.User).Include(t => t.AssignedTo)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new SupportTicketDto { Id = t.Id, UserId = t.UserId, UserName = t.User.UserName!, Subject = t.Subject, Description = t.Description, Priority = t.Priority, Status = t.Status, AssignedToUserId = t.AssignedToUserId, Resolution = t.Resolution, CreatedAt = t.CreatedAt, ResolvedAt = t.ResolvedAt })
            .ToListAsync();

    public async Task<List<SupportTicketDto>> GetUserTicketsAsync(string userId)
        => await _db.SupportTickets.Where(t => t.UserId == userId)
            .Include(t => t.User)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new SupportTicketDto { Id = t.Id, UserId = t.UserId, UserName = t.User.UserName!, Subject = t.Subject, Description = t.Description, Priority = t.Priority, Status = t.Status, Resolution = t.Resolution, CreatedAt = t.CreatedAt, ResolvedAt = t.ResolvedAt })
            .ToListAsync();

    public async Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto, string userId)
    {
        var t = new SupportTicket { UserId = userId, Subject = dto.Subject, Description = dto.Description, Priority = dto.Priority };
        _db.SupportTickets.Add(t);
        await _db.SaveChangesAsync();
        var user = await _db.Users.FindAsync(userId);
        return new SupportTicketDto { Id = t.Id, UserId = t.UserId, UserName = user?.UserName ?? userId, Subject = t.Subject, Description = t.Description, Priority = t.Priority, Status = t.Status, CreatedAt = t.CreatedAt };
    }

    public async Task<bool> ResolveAsync(int id, string resolution)
    {
        var t = await _db.SupportTickets.FindAsync(id);
        if (t == null) return false;
        t.Resolution = resolution; t.Status = Enums.TicketStatus.Resolved; t.ResolvedAt = DateTime.UtcNow; t.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }
}
