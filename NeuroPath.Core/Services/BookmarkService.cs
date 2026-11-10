using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Services;

public class BookmarkService : IBookmarkService
{
    private readonly INeuroPathDbContext _db;

    public BookmarkService(INeuroPathDbContext db) => _db = db;

    public async Task<List<BookmarkDto>> GetUserBookmarksAsync(string userId, int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        var rows = await _db.Bookmarks.AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .Skip(skip).Take(take)
            .ToListAsync();
        return rows.Select(ToDto).ToList();
    }

    public async Task<BookmarkDto> CreateAsync(CreateBookmarkDto dto, string userId)
    {
        if (!Enum.TryParse<BookmarkType>(dto.BookmarkType, ignoreCase: true, out var type) || !Enum.IsDefined(type))
            throw new BusinessRuleException("Invalid bookmark type.");

        if (!await ReferenceExistsAsync(type, dto.ReferenceId))
            throw new NotFoundException($"{type} {dto.ReferenceId} was not found.");

        if (await _db.Bookmarks.AnyAsync(b => b.UserId == userId && b.BookmarkType == type && b.ReferenceId == dto.ReferenceId))
            throw new ConflictException("This item is already bookmarked.");

        var bookmark = new Bookmark { UserId = userId, BookmarkType = type, ReferenceId = dto.ReferenceId, Note = dto.Note };
        _db.Bookmarks.Add(bookmark);
        await _db.SaveChangesAsync();
        return ToDto(bookmark);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
        => await _db.Bookmarks.Where(b => b.Id == id && b.UserId == userId).ExecuteDeleteAsync() > 0;

    private Task<bool> ReferenceExistsAsync(BookmarkType type, int id) => type switch
    {
        BookmarkType.Exercise => _db.Exercises.AnyAsync(e => e.Id == id && e.IsActive),
        BookmarkType.Article => _db.Articles.AnyAsync(a => a.Id == id && a.IsPublished),
        BookmarkType.ResearchStudy => _db.ResearchStudies.AnyAsync(r => r.Id == id),
        BookmarkType.Quiz => _db.Quizzes.AnyAsync(q => q.Id == id && q.IsPublished && q.IsActive),
        BookmarkType.Course => _db.CourseModules.AnyAsync(c => c.Id == id && c.IsPublished),
        _ => Task.FromResult(false)
    };

    private static BookmarkDto ToDto(Bookmark b) => new()
    {
        Id = b.Id,
        BookmarkType = b.BookmarkType.ToString(),
        ReferenceId = b.ReferenceId,
        Note = b.Note,
        CreatedAt = b.CreatedAt
    };
}
