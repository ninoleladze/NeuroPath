using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeuroPath.Core.Common;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.Auth;
using NeuroPath.Core.Security;
using NeuroPath.Core.Settings;

namespace NeuroPath.Core.Services;

public class AuthService : IAuthService
{
    private static readonly TimeSpan VerificationTokenLifetime = TimeSpan.FromHours(24);
    private static readonly TimeSpan PasswordResetTokenLifetime = TimeSpan.FromHours(1);
    private static readonly TimeSpan RefreshReuseGracePeriod = TimeSpan.FromSeconds(20);

    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwt;
    private readonly INeuroPathDbContext _db;
    private readonly IEmailService _email;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<User> userManager, IOptions<JwtSettings> jwt, INeuroPathDbContext db, IEmailService email, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
        _db = db;
        _email = email;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, string? ipAddress)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            throw new ConflictException("Email is already registered.");
        if (await _userManager.FindByNameAsync(dto.UserName) != null)
            throw new ConflictException("Username is already taken.");

        var user = new User { UserName = dto.UserName, Email = dto.Email, CreatedAt = DateTime.UtcNow };
        var verificationToken = TokenHasher.NewToken();

        await using (var transaction = await _db.Database.BeginTransactionAsync())
        {
            EnsureSucceeded(await _userManager.CreateAsync(user, dto.Password));
            EnsureSucceeded(await _userManager.AddToRoleAsync(user, AppRoles.User));

            _db.UserProfiles.Add(new UserProfile { UserId = user.Id, FirstName = dto.FirstName, LastName = dto.LastName });
            _db.UserSettings.Add(new UserSettings { UserId = user.Id });
            _db.Streaks.Add(new Streak { UserId = user.Id, LastActivityDate = DateTime.UtcNow });
            _db.EmailVerificationTokens.Add(new EmailVerificationToken
            {
                UserId = user.Id,
                Token = TokenHasher.Hash(verificationToken),
                ExpiresAt = DateTime.UtcNow.Add(VerificationTokenLifetime)
            });

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        await _email.SendVerificationEmailAsync(user.Email!, dto.FirstName, verificationToken);
        return new AuthResponseDto { Message = "Registration successful. Please check your email to verify your account." };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, string? ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (await _userManager.IsLockedOutAsync(user))
            throw new UnauthorizedAccessException("Too many failed attempts. Try again later.");

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            await _userManager.AccessFailedAsync(user);
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        if (!user.IsEmailVerified) throw new UnauthorizedAccessException("Please verify your email first.");
        if (user.IsBlocked || !user.IsActive) throw new UnauthorizedAccessException("Account is not available.");

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await IssueTokensAsync(user, ipAddress, NewRefreshToken());
    }

    public async Task<AuthResponseDto> RefreshAsync(string refreshToken, string? ipAddress)
    {
        var now = DateTime.UtcNow;
        var hash = TokenHasher.Hash(refreshToken);
        var stored = await _db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.TokenHash == hash)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (stored.ExpiresAt <= now)
            throw new UnauthorizedAccessException("Refresh token has expired.");

        var user = await _userManager.FindByIdAsync(stored.UserId)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");
        if (user.IsBlocked || !user.IsActive)
            throw new UnauthorizedAccessException("Account is not available.");

        var next = NewRefreshToken();

        if (stored.RevokedAt == null)
        {
            var claimed = await _db.RefreshTokens
                .Where(t => t.Id == stored.Id && t.RevokedAt == null)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.RevokedAt, now)
                    .SetProperty(t => t.ReplacedByTokenHash, next.Hash));
            if (claimed == 1)
                return await IssueTokensAsync(user, ipAddress, next);

            stored = await _db.RefreshTokens.AsNoTracking().FirstAsync(t => t.Id == stored.Id);
        }

        var rotatedMomentsAgo = stored.ReplacedByTokenHash != null && stored.RevokedAt >= now - RefreshReuseGracePeriod;
        if (rotatedMomentsAgo)
            return await IssueTokensAsync(user, ipAddress, next);

        await RevokeAllRefreshTokensAsync(user.Id);
        await _userManager.UpdateSecurityStampAsync(user);
        _logger.LogWarning("Refresh token reuse detected for user {UserId}; all sessions were revoked.", user.Id);
        throw new UnauthorizedAccessException("Invalid refresh token.");
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var hash = TokenHasher.Hash(token);
        var stored = await _db.EmailVerificationTokens.Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == hash && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);
        if (stored == null) return false;

        stored.IsUsed = true;
        stored.UsedAt = DateTime.UtcNow;
        stored.User.IsEmailVerified = true;
        stored.User.EmailConfirmed = true;
        stored.User.EmailVerifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        await _email.SendWelcomeEmailAsync(stored.User.Email!, await GetFirstNameAsync(stored.UserId));
        return true;
    }

    public async Task ResendVerificationEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.IsEmailVerified) return;

        await _db.EmailVerificationTokens
            .Where(t => t.UserId == user.Id && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.ExpiresAt, DateTime.UtcNow));

        var token = TokenHasher.NewToken();
        _db.EmailVerificationTokens.Add(new EmailVerificationToken
        {
            UserId = user.Id,
            Token = TokenHasher.Hash(token),
            ExpiresAt = DateTime.UtcNow.Add(VerificationTokenLifetime)
        });
        await _db.SaveChangesAsync();

        await _email.SendVerificationEmailAsync(user.Email!, await GetFirstNameAsync(user.Id), token);
    }

    public async Task ForgotPasswordAsync(string email, string? ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        await _db.PasswordResetTokens
            .Where(t => t.UserId == user.Id && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.ExpiresAt, DateTime.UtcNow));

        var token = TokenHasher.NewToken();
        _db.PasswordResetTokens.Add(new PasswordResetToken
        {
            UserId = user.Id,
            Token = TokenHasher.Hash(token),
            ExpiresAt = DateTime.UtcNow.Add(PasswordResetTokenLifetime),
            IpAddress = ipAddress
        });
        await _db.SaveChangesAsync();

        await _email.SendPasswordResetEmailAsync(user.Email!, await GetFirstNameAsync(user.Id), token);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var hash = TokenHasher.Hash(dto.Token);
        var stored = await _db.PasswordResetTokens.Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == hash && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);
        if (stored == null) return false;

        var identityToken = await _userManager.GeneratePasswordResetTokenAsync(stored.User);
        var result = await _userManager.ResetPasswordAsync(stored.User, identityToken, dto.NewPassword);
        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(" ", result.Errors.Select(e => e.Description)));

        stored.IsUsed = true;
        stored.UsedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await RevokeAllRefreshTokensAsync(stored.UserId);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(" ", result.Errors.Select(e => e.Description)));

        await RevokeAllRefreshTokensAsync(userId);
        return true;
    }

    public async Task LogoutAsync(string userId)
    {
        await RevokeAllRefreshTokensAsync(userId);
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null) await _userManager.UpdateSecurityStampAsync(user);
    }

    private static (string Token, string Hash) NewRefreshToken()
    {
        var token = TokenHasher.NewToken();
        return (token, TokenHasher.Hash(token));
    }

    private async Task<AuthResponseDto> IssueTokensAsync(User user, string? ipAddress, (string Token, string Hash) refresh)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, expiresAt) = CreateAccessToken(user, roles);

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refresh.Hash,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays),
            CreatedByIp = ipAddress
        });
        await _db.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiresAt = expiresAt,
            RefreshToken = refresh.Token,
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
            AvatarUrl = user.AvatarUrl
        };
    }

    private (string Token, DateTime ExpiresAt) CreateAccessToken(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(AppClaimTypes.SecurityStamp, user.SecurityStamp ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var token = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    private async Task RevokeAllRefreshTokensAsync(string userId)
        => await _db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow));

    private async Task<string> GetFirstNameAsync(string userId)
        => await _db.UserProfiles.Where(p => p.UserId == userId).Select(p => p.FirstName).FirstOrDefaultAsync() ?? "there";

    private static void EnsureSucceeded(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new BusinessRuleException(string.Join(" ", result.Errors.Select(e => e.Description)));
    }
}
