using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Interfaces;

public interface IBookmarkService
{
    Task<List<BookmarkDto>> GetUserBookmarksAsync(string userId, int page = 1, int pageSize = 20);
    Task<BookmarkDto> CreateAsync(CreateBookmarkDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}
