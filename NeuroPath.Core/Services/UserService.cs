using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.User;

namespace NeuroPath.Core.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly INeuroPathDbContext _db;
    private readonly ITeamService _teams;
    private readonly IFileUploadService _files;

    public UserService(UserManager<User> userManager, INeuroPathDbContext db, ITeamService teams, IFileUploadService files)
    {
        _userManager = userManager;
        _db = db;
        _teams = teams;
        _files = files;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var users = await LoadUsersAsync(_db.Users.Where(u => u.Id == id));
        return users.FirstOrDefault();
    }

    public async Task<UserProfileDto?> GetProfileAsync(string userId)
        => await _db.UserProfiles.AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => new UserProfileDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Bio = p.Bio,
                DateOfBirth = p.DateOfBirth,
                Country = p.Country,
                City = p.City,
                Occupation = p.Occupation,
                CognitiveGoal = p.CognitiveGoal,
                DailyTrainingMinutes = p.DailyTrainingMinutes
            })
            .FirstOrDefaultAsync();

    public async Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return false;

        profile.FirstName = dto.FirstName;
        profile.LastName = dto.LastName;
        profile.Bio = dto.Bio;
        profile.DateOfBirth = dto.DateOfBirth;
        profile.Country = dto.Country;
        profile.City = dto.City;
        profile.Occupation = dto.Occupation;
        profile.CognitiveGoal = dto.CognitiveGoal;
        profile.DailyTrainingMinutes = dto.DailyTrainingMinutes;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<UserSettingsDto?> GetSettingsAsync(string userId)
        => await _db.UserSettings.AsNoTracking()
            .Where(s => s.UserId == userId)
            .Select(s => new UserSettingsDto
            {
                Language = s.Language,
                Theme = s.Theme,
                EmailNotifications = s.EmailNotifications,
                PushNotifications = s.PushNotifications,
                StreakReminders = s.StreakReminders,
                WeeklySummary = s.WeeklySummary,
                ProfileVisibility = s.ProfileVisibility,
                ShowOnLeaderboard = s.ShowOnLeaderboard,
                TimeZoneId = s.TimeZoneId
            })
            .FirstOrDefaultAsync();

    public async Task<bool> UpdateSettingsAsync(string userId, UpdateSettingsDto dto)
    {
        var settings = await _db.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
        if (settings == null) return false;

        settings.Language = dto.Language;
        settings.Theme = dto.Theme;
        settings.EmailNotifications = dto.EmailNotifications;
        settings.PushNotifications = dto.PushNotifications;
        settings.StreakReminders = dto.StreakReminders;
        settings.WeeklySummary = dto.WeeklySummary;
        settings.ProfileVisibility = dto.ProfileVisibility;
        settings.ShowOnLeaderboard = dto.ShowOnLeaderboard;
        settings.TimeZoneId = dto.TimeZoneId;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDto>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        return await LoadUsersAsync(_db.Users.OrderBy(u => u.UserName).Skip(skip).Take(take));
    }

    public async Task<bool> SetBlockedAsync(string userId, bool isBlocked, string actingAdminId)
    {
        if (userId == actingAdminId)
            throw new BusinessRuleException("You cannot block your own account.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        if (user.IsBlocked == isBlocked) return true;

        user.IsBlocked = isBlocked;
        EnsureSucceeded(await _userManager.UpdateAsync(user));
        EnsureSucceeded(await _userManager.UpdateSecurityStampAsync(user));

        if (isBlocked)
            await _db.RefreshTokens
                .Where(t => t.UserId == userId && t.RevokedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow));
        return true;
    }

    public async Task<bool> SetRoleAsync(string userId, string role, string actingAdminId)
    {
        var normalizedRole = AppRoles.All.FirstOrDefault(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase))
            ?? throw new BusinessRuleException($"Role must be one of: {string.Join(", ", AppRoles.All)}.");
        if (userId == actingAdminId)
            throw new BusinessRuleException("You cannot change your own role.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        await using var transaction = await _db.Database.BeginTransactionAsync();
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
            EnsureSucceeded(await _userManager.RemoveFromRolesAsync(user, currentRoles));
        EnsureSucceeded(await _userManager.AddToRoleAsync(user, normalizedRole));
        EnsureSucceeded(await _userManager.UpdateSecurityStampAsync(user));
        await transaction.CommitAsync();
        return true;
    }

    public async Task<bool> DeleteAccountAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !user.IsActive) return false;
        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new BusinessRuleException("The password is incorrect.");

        if (await _userManager.IsInRoleAsync(user, AppRoles.Admin) && (await _userManager.GetUsersInRoleAsync(AppRoles.Admin)).Count(a => a.IsActive) <= 1)
            throw new BusinessRuleException("The last administrator cannot delete their account.");

        foreach (var teamId in await _db.TeamMembers.Where(m => m.UserId == userId).Select(m => m.TeamId).ToListAsync())
            await _teams.LeaveAsync(teamId, userId);
        foreach (var fileId in await _db.UploadedFiles.Where(f => f.UserId == userId).Select(f => f.Id).ToListAsync())
            await _files.DeleteFileAsync(fileId, userId);

        await using var transaction = await _db.Database.BeginTransactionAsync();

        await _db.Friendships.Where(f => f.UserId == userId || f.FriendId == userId).ExecuteDeleteAsync();
        await _db.FriendRequests.Where(r => r.SenderId == userId || r.ReceiverId == userId).ExecuteDeleteAsync();
        await _db.Notifications.Where(n => n.UserId == userId).ExecuteDeleteAsync();
        await _db.Bookmarks.Where(b => b.UserId == userId).ExecuteDeleteAsync();
        await _db.MoodLogs.Where(m => m.UserId == userId).ExecuteDeleteAsync();
        await _db.RefreshTokens.Where(t => t.UserId == userId).ExecuteDeleteAsync();
        await _db.EmailVerificationTokens.Where(t => t.UserId == userId).ExecuteDeleteAsync();
        await _db.PasswordResetTokens.Where(t => t.UserId == userId).ExecuteDeleteAsync();
        await _db.UserProfiles.Where(p => p.UserId == userId).ExecuteUpdateAsync(s => s
            .SetProperty(p => p.FirstName, "Deleted")
            .SetProperty(p => p.LastName, "User")
            .SetProperty(p => p.Bio, (string?)null)
            .SetProperty(p => p.DateOfBirth, (DateTime?)null)
            .SetProperty(p => p.Country, (string?)null)
            .SetProperty(p => p.City, (string?)null)
            .SetProperty(p => p.Occupation, (string?)null)
            .SetProperty(p => p.CognitiveGoal, (string?)null));
        await _db.UserSettings.Where(s => s.UserId == userId).ExecuteUpdateAsync(s => s
            .SetProperty(x => x.ShowOnLeaderboard, false)
            .SetProperty(x => x.EmailNotifications, false)
            .SetProperty(x => x.PushNotifications, false)
            .SetProperty(x => x.StreakReminders, false)
            .SetProperty(x => x.WeeklySummary, false)
            .SetProperty(x => x.ProfileVisibility, "private"));

        var anonymousId = user.Id.Replace("-", string.Empty);
        user.UserName = $"deleted-{anonymousId}";
        user.Email = $"{anonymousId}@deleted.invalid";
        user.PhoneNumber = null;
        user.AvatarUrl = null;
        user.IsActive = false;
        user.EmailConfirmed = false;
        user.IsEmailVerified = false;
        EnsureSucceeded(await _userManager.UpdateAsync(user));
        EnsureSucceeded(await _userManager.RemovePasswordAsync(user));
        EnsureSucceeded(await _userManager.UpdateSecurityStampAsync(user));

        await transaction.CommitAsync();
        return true;
    }

    private async Task<List<UserDto>> LoadUsersAsync(IQueryable<User> query)
    {
        var users = await query.AsNoTracking()
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                IsActive = u.IsActive,
                IsBlocked = u.IsBlocked,
                IsEmailVerified = u.IsEmailVerified,
                AvatarUrl = u.AvatarUrl,
                TotalPoints = u.TotalPoints,
                CurrentBalance = u.CurrentBalance,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
        if (users.Count == 0) return users;

        var ids = users.Select(u => u.Id).ToList();
        var roles = await _db.UserRoles.AsNoTracking()
            .Where(ur => ids.Contains(ur.UserId))
            .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
            .ToListAsync();
        var rolesByUser = roles.ToLookup(r => r.UserId, r => r.Name!);

        foreach (var user in users)
            user.Roles = rolesByUser[user.Id].ToList();
        return users;
    }

    private static void EnsureSucceeded(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(" ", result.Errors.Select(e => e.Description)));
    }
}
