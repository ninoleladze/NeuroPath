using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class SupportTicketService : ISupportTicketService
{
    private readonly INeuroPathDbContext _db;

    public SupportTicketService(INeuroPathDbContext db) => _db = db;

    public async Task<List<SupportTicketDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.SupportTickets.AsNoTracking()
            .OrderBy(t => t.Status).ThenByDescending(t => t.Priority).ThenBy(t => t.CreatedAt)
            .Skip(skip).Take(take)
            .Select(t => new SupportTicketDto
            {
                Id = t.Id,
                UserId = t.UserId,
                UserName = t.User.UserName!,
                Subject = t.Subject,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                AssignedToUserId = t.AssignedToUserId,
                Resolution = t.Resolution,
                CreatedAt = t.CreatedAt,
                ResolvedAt = t.ResolvedAt
            })
            .ToListAsync();
    }

    public async Task<List<SupportTicketDto>> GetUserTicketsAsync(string userId)
        => await _db.SupportTickets.AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new SupportTicketDto
            {
                Id = t.Id,
                UserId = t.UserId,
                UserName = t.User.UserName!,
                Subject = t.Subject,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                Resolution = t.Resolution,
                CreatedAt = t.CreatedAt,
                ResolvedAt = t.ResolvedAt
            })
            .ToListAsync();

    public async Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto, string userId)
    {
        var ticket = new SupportTicket { UserId = userId, Subject = dto.Subject, Description = dto.Description, Priority = dto.Priority };
        _db.SupportTickets.Add(ticket);
        await _db.SaveChangesAsync();

        var userName = await _db.Users.Where(u => u.Id == userId).Select(u => u.UserName).FirstOrDefaultAsync();
        return new SupportTicketDto
        {
            Id = ticket.Id,
            UserId = userId,
            UserName = userName ?? string.Empty,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Priority = ticket.Priority,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt
        };
    }

    public async Task<bool> AssignAsync(int id, string assigneeUserId)
    {
        if (!await _db.Users.AnyAsync(u => u.Id == assigneeUserId))
            throw new NotFoundException("Assignee not found.");

        return await _db.SupportTickets
            .Where(t => t.Id == id && t.Status != TicketStatus.Closed)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.AssignedToUserId, assigneeUserId)
                .SetProperty(t => t.Status, TicketStatus.InProgress)
                .SetProperty(t => t.UpdatedAt, DateTime.UtcNow)) > 0;
    }

    public async Task<bool> ResolveAsync(int id, string resolution, string resolverUserId)
        => await _db.SupportTickets
            .Where(t => t.Id == id && t.Status != TicketStatus.Closed)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Resolution, resolution)
                .SetProperty(t => t.Status, TicketStatus.Resolved)
                .SetProperty(t => t.AssignedToUserId, t => t.AssignedToUserId ?? resolverUserId)
                .SetProperty(t => t.ResolvedAt, DateTime.UtcNow)
                .SetProperty(t => t.UpdatedAt, DateTime.UtcNow)) > 0;
}
