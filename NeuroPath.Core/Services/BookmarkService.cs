using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class BookmarkService : IBookmarkService
{
    private readonly NeuroPathDbContext _db;

    public BookmarkService(NeuroPathDbContext db) => _db = db;

    public async Task<List<BookmarkDto>> GetUserBookmarksAsync(string userId)
        => await _db.Bookmarks.Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BookmarkDto
            {
                Id = b.Id, BookmarkType = b.BookmarkType.ToString(),
                ReferenceId = b.ReferenceId, Note = b.Note, CreatedAt = b.CreatedAt
            }).ToListAsync();

    public async Task<BookmarkDto> CreateAsync(CreateBookmarkDto dto, string userId)
    {
        if (!Enum.TryParse<BookmarkType>(dto.BookmarkType, out var type))
            throw new InvalidOperationException("Invalid bookmark type.");
        var b = new Bookmark { UserId = userId, BookmarkType = type, ReferenceId = dto.ReferenceId, Note = dto.Note };
        _db.Bookmarks.Add(b);
        await _db.SaveChangesAsync();
        return new BookmarkDto { Id = b.Id, BookmarkType = b.BookmarkType.ToString(), ReferenceId = b.ReferenceId, Note = b.Note, CreatedAt = b.CreatedAt };
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var b = await _db.Bookmarks.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (b == null) return false;
        _db.Bookmarks.Remove(b);
        await _db.SaveChangesAsync();
        return true;
    }
}
