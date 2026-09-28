using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.Auth;
using NeuroPath.Core.Settings;
using NeuroPath.Data.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NeuroPath.Core.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _um;
    private readonly JwtSettings _jwt;
    private readonly NeuroPathDbContext _db;
    private readonly IEmailService _email;

    public AuthService(UserManager<User> um, IOptions<JwtSettings> jwt, NeuroPathDbContext db, IEmailService email)
    {
        _um = um;
        _jwt = jwt.Value;
        _db = db;
        _email = email;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, string ipAddress)
    {
        if (await _um.FindByEmailAsync(dto.Email) != null) throw new InvalidOperationException("Email already registered.");
        if (await _um.FindByNameAsync(dto.UserName) != null) throw new InvalidOperationException("Username already taken.");
        var user = new User { UserName = dto.UserName, Email = dto.Email, CreatedAt = DateTime.UtcNow };
        var result = await _um.CreateAsync(user, dto.Password);
        if (!result.Succeeded) throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        await _um.AddToRoleAsync(user, "User");
        _db.UserProfiles.Add(new UserProfile { UserId = user.Id, FirstName = dto.FirstName, LastName = dto.LastName });
        _db.UserSettings.Add(new UserSettings { UserId = user.Id });
        _db.Streaks.Add(new Streak { UserId = user.Id, LastActivityDate = DateTime.UtcNow });
        var token = Guid.NewGuid().ToString("N");
        _db.EmailVerificationTokens.Add(new EmailVerificationToken { UserId = user.Id, Token = token, ExpiresAt = DateTime.UtcNow.AddHours(24) });
        await _db.SaveChangesAsync();
        await _email.SendVerificationEmailAsync(user.Email!, dto.FirstName, token);
        return new AuthResponseDto { Message = "Registration successful. Please check your email to verify your account." };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, string ipAddress)
    {
        var user = await _um.FindByEmailAsync(dto.Email) ?? throw new UnauthorizedAccessException("Invalid credentials.");
        if (!await _um.CheckPasswordAsync(user, dto.Password)) throw new UnauthorizedAccessException("Invalid credentials.");
        if (!user.IsEmailVerified) throw new UnauthorizedAccessException("Please verify your email first.");
        if (user.IsBlocked) throw new UnauthorizedAccessException("Account is blocked.");
        user.LastLoginAt = DateTime.UtcNow;
        await _um.UpdateAsync(user);
        var roles = await _um.GetRolesAsync(user);
        return new AuthResponseDto
        {
            AccessToken = GenerateToken(user, roles),
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Roles = roles.ToList(),
            AvatarUrl = user.AvatarUrl
        };
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var t = await _db.EmailVerificationTokens.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow);
        if (t == null) return false;
        t.IsUsed = true;
        t.UsedAt = DateTime.UtcNow;
        t.User.IsEmailVerified = true;
        t.User.EmailVerifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        var p = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == t.UserId);
        await _email.SendWelcomeEmailAsync(t.User.Email!, p?.FirstName ?? "User");
        return true;
    }

    public async Task<bool> ResendVerificationEmailAsync(string email)
    {
        var user = await _um.FindByEmailAsync(email);
        if (user == null || user.IsEmailVerified) return false;
        var token = Guid.NewGuid().ToString("N");
        _db.EmailVerificationTokens.Add(new EmailVerificationToken { UserId = user.Id, Token = token, ExpiresAt = DateTime.UtcNow.AddHours(24) });
        await _db.SaveChangesAsync();
        var p = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
        await _email.SendVerificationEmailAsync(user.Email!, p?.FirstName ?? "User", token);
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(string email, string ipAddress)
    {
        var user = await _um.FindByEmailAsync(email);
        if (user == null) return true;
        var token = Guid.NewGuid().ToString("N");
        _db.PasswordResetTokens.Add(new PasswordResetToken { UserId = user.Id, Token = token, ExpiresAt = DateTime.UtcNow.AddHours(1), IpAddress = ipAddress });
        await _db.SaveChangesAsync();
        var p = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
        await _email.SendPasswordResetEmailAsync(user.Email!, p?.FirstName ?? "User", token);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var t = await _db.PasswordResetTokens.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == dto.Token && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow);
        if (t == null) return false;
        var resetToken = await _um.GeneratePasswordResetTokenAsync(t.User);
        var result = await _um.ResetPasswordAsync(t.User, resetToken, dto.NewPassword);
        if (!result.Succeeded) return false;
        t.IsUsed = true;
        t.UsedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _um.FindByIdAsync(userId);
        if (user == null) return false;
        var result = await _um.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        return result.Succeeded;
    }

    public Task LogoutAsync(string userId) => Task.CompletedTask;

    private string GenerateToken(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
