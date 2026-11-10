using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class FeedbackService : IFeedbackService
{
    private readonly INeuroPathDbContext _db;

    public FeedbackService(INeuroPathDbContext db) => _db = db;

    public async Task<List<FeedbackDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await _db.Feedbacks.AsNoTracking()
            .OrderByDescending(f => f.CreatedAt)
            .Skip(skip).Take(take)
            .Select(f => new FeedbackDto
            {
                Id = f.Id,
                UserId = f.UserId,
                UserName = f.User.UserName!,
                Subject = f.Subject,
                Message = f.Message,
                Rating = f.Rating,
                Category = f.Category,
                IsReviewed = f.IsReviewed,
                AdminResponse = f.AdminResponse,
                CreatedAt = f.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto, string userId)
    {
        var feedback = new Feedback { UserId = userId, Subject = dto.Subject, Message = dto.Message, Rating = dto.Rating, Category = dto.Category };
        _db.Feedbacks.Add(feedback);
        await _db.SaveChangesAsync();

        var userName = await _db.Users.Where(u => u.Id == userId).Select(u => u.UserName).FirstOrDefaultAsync();
        return new FeedbackDto
        {
            Id = feedback.Id,
            UserId = userId,
            UserName = userName ?? string.Empty,
            Subject = feedback.Subject,
            Message = feedback.Message,
            Rating = feedback.Rating,
            Category = feedback.Category,
            IsReviewed = feedback.IsReviewed,
            CreatedAt = feedback.CreatedAt
        };
    }

    public async Task<bool> RespondAsync(int id, string response)
        => await _db.Feedbacks.Where(f => f.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(f => f.AdminResponse, response)
                .SetProperty(f => f.IsReviewed, true)
                .SetProperty(f => f.ReviewedAt, DateTime.UtcNow)) > 0;
}
