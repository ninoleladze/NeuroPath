using NeuroPath.Core.Models.DTOs.User;

namespace NeuroPath.Core.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserProfileDto?> GetProfileAsync(string userId);
    Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task<UserSettingsDto?> GetSettingsAsync(string userId);
    Task<bool> UpdateSettingsAsync(string userId, UpdateSettingsDto dto);
    Task<List<UserDto>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<bool> SetBlockedAsync(string userId, bool isBlocked, string actingAdminId);
    Task<bool> SetRoleAsync(string userId, string role, string actingAdminId);
    Task<bool> DeleteAccountAsync(string userId, string password);
}
