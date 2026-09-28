using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.User;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _um;
    private readonly NeuroPathDbContext _db;

    public UserService(UserManager<User> um, NeuroPathDbContext db)
    {
        _um = um;
        _db = db;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _um.FindByIdAsync(id);
        if (user == null) return null;
        var roles = await _um.GetRolesAsync(user);
        return new UserDto
        {
            Id = user.Id, UserName = user.UserName!, Email = user.Email!, IsActive = user.IsActive,
            IsEmailVerified = user.IsEmailVerified, AvatarUrl = user.AvatarUrl,
            TotalPoints = user.TotalPoints, CurrentBalance = user.CurrentBalance,
            CreatedAt = user.CreatedAt, Roles = roles.ToList()
        };
    }

    public async Task<UserProfileDto?> GetProfileAsync(string userId)
    {
        var p = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (p == null) return null;
        return new UserProfileDto
        {
            FirstName = p.FirstName, LastName = p.LastName, Bio = p.Bio, DateOfBirth = p.DateOfBirth,
            Country = p.Country, City = p.City, Occupation = p.Occupation,
            CognitiveGoal = p.CognitiveGoal, DailyTrainingMinutes = p.DailyTrainingMinutes
        };
    }

    public async Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var p = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (p == null) return false;
        p.FirstName = dto.FirstName; p.LastName = dto.LastName; p.Bio = dto.Bio;
        p.DateOfBirth = dto.DateOfBirth; p.Country = dto.Country; p.City = dto.City;
        p.Occupation = dto.Occupation; p.CognitiveGoal = dto.CognitiveGoal;
        p.DailyTrainingMinutes = dto.DailyTrainingMinutes;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<UserSettingsDto?> GetSettingsAsync(string userId)
    {
        var s = await _db.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId);
        if (s == null) return null;
        return new UserSettingsDto
        {
            Language = s.Language, Theme = s.Theme, EmailNotifications = s.EmailNotifications,
            PushNotifications = s.PushNotifications, StreakReminders = s.StreakReminders,
            WeeklySummary = s.WeeklySummary, ProfileVisibility = s.ProfileVisibility,
            ShowOnLeaderboard = s.ShowOnLeaderboard
        };
    }

    public async Task<bool> UpdateSettingsAsync(string userId, UpdateSettingsDto dto)
    {
        var s = await _db.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId);
        if (s == null) return false;
        s.Language = dto.Language; s.Theme = dto.Theme; s.EmailNotifications = dto.EmailNotifications;
        s.PushNotifications = dto.PushNotifications; s.StreakReminders = dto.StreakReminders;
        s.WeeklySummary = dto.WeeklySummary; s.ProfileVisibility = dto.ProfileVisibility;
        s.ShowOnLeaderboard = dto.ShowOnLeaderboard;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _db.Users.ToListAsync();
        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _um.GetRolesAsync(user);
            result.Add(new UserDto
            {
                Id = user.Id, UserName = user.UserName!, Email = user.Email!, IsActive = user.IsActive,
                IsEmailVerified = user.IsEmailVerified, AvatarUrl = user.AvatarUrl,
                TotalPoints = user.TotalPoints, CurrentBalance = user.CurrentBalance,
                CreatedAt = user.CreatedAt, Roles = roles.ToList()
            });
        }
        return result;
    }

    public async Task<bool> BlockUserAsync(string userId)
    {
        var user = await _um.FindByIdAsync(userId);
        if (user == null) return false;
        user.IsBlocked = !user.IsBlocked;
        await _um.UpdateAsync(user);
        return true;
    }

    public async Task<bool> SetRoleAsync(string userId, string role)
    {
        var user = await _um.FindByIdAsync(userId);
        if (user == null) return false;
        var currentRoles = await _um.GetRolesAsync(user);
        await _um.RemoveFromRolesAsync(user, currentRoles);
        var result = await _um.AddToRoleAsync(user, role);
        return result.Succeeded;
    }
}
