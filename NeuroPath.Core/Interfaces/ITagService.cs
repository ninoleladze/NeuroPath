using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Interfaces;

public interface ITagService
{
    Task<List<TagDto>> GetAllAsync();
    Task<TagDto> CreateAsync(CreateTagDto dto);
}
