using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class FeedbackService : IFeedbackService
{
    private readonly NeuroPathDbContext _db;

    public FeedbackService(NeuroPathDbContext db) => _db = db;

    public async Task<List<FeedbackDto>> GetAllAsync()
        => await _db.Feedbacks.Include(f => f.User)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FeedbackDto { Id = f.Id, UserId = f.UserId, UserName = f.User.UserName!, Subject = f.Subject, Message = f.Message, Rating = f.Rating, Category = f.Category, IsReviewed = f.IsReviewed, AdminResponse = f.AdminResponse, CreatedAt = f.CreatedAt })
            .ToListAsync();

    public async Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto, string userId)
    {
        var f = new Feedback { UserId = userId, Subject = dto.Subject, Message = dto.Message, Rating = dto.Rating, Category = dto.Category };
        _db.Feedbacks.Add(f);
        await _db.SaveChangesAsync();
        var user = await _db.Users.FindAsync(userId);
        return new FeedbackDto { Id = f.Id, UserId = f.UserId, UserName = user?.UserName ?? userId, Subject = f.Subject, Message = f.Message, Rating = f.Rating, Category = f.Category, IsReviewed = f.IsReviewed, CreatedAt = f.CreatedAt };
    }

    public async Task<bool> RespondAsync(int id, string response)
    {
        var f = await _db.Feedbacks.FindAsync(id);
        if (f == null) return false;
        f.AdminResponse = response; f.IsReviewed = true; f.ReviewedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }
}
