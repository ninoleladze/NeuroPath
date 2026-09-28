using NeuroPath.Core.Models.DTOs.User;

namespace NeuroPath.Core.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserProfileDto?> GetProfileAsync(string userId);
    Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task<UserSettingsDto?> GetSettingsAsync(string userId);
    Task<bool> UpdateSettingsAsync(string userId, UpdateSettingsDto dto);
    Task<List<UserDto>> GetAllAsync();
    Task<bool> BlockUserAsync(string userId);
    Task<bool> SetRoleAsync(string userId, string role);
}
