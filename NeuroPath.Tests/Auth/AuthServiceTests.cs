using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.DTOs.Auth;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Auth;

public class AuthServiceTests
{
    private const string Email = "player@neuropath.test";

    [Fact]
    public async Task Register_CreatesUnverifiedUserWithProfileSettingsAndStreak()
    {
        using var host = await IdentityTestHost.CreateAsync();

        var response = await host.RegisterAsync(Email);

        Assert.Null(response.AccessToken);
        Assert.True(host.Email.VerificationTokens.ContainsKey(Email));
        await using var db = host.NewContext();
        var user = await db.Users.SingleAsync(u => u.Email == Email);
        Assert.False(user.IsEmailVerified);
        Assert.True(await db.UserProfiles.AnyAsync(p => p.UserId == user.Id));
        Assert.True(await db.UserSettings.AnyAsync(s => s.UserId == user.Id && s.TimeZoneId == "UTC"));
        Assert.True(await db.Streaks.AnyAsync(s => s.UserId == user.Id));
        Assert.Equal(1, await db.UserRoles.CountAsync(r => r.UserId == user.Id));
    }

    [Fact]
    public async Task Register_SameEmailInDifferentCase_IsConflict()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterAsync(Email);

        await Assert.ThrowsAsync<ConflictException>(() => host.RunAsync(auth => auth.RegisterAsync(new RegisterDto
        {
            UserName = "another",
            Email = Email.ToUpperInvariant(),
            Password = IdentityTestHost.Password,
            FirstName = "A",
            LastName = "B"
        }, null)));
    }

    [Fact]
    public async Task Login_BeforeVerification_IsRejected_AndWorksAfterwards()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterAsync(Email);

        var before = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.LoginAsync(Email));
        Assert.Contains("verify", before.Message, StringComparison.OrdinalIgnoreCase);

        Assert.True(await host.RunAsync(auth => auth.VerifyEmailAsync(host.Email.VerificationTokens[Email])));
        var login = await host.LoginAsync(Email);

        Assert.False(string.IsNullOrEmpty(login.AccessToken));
        Assert.False(string.IsNullOrEmpty(login.RefreshToken));
        Assert.Contains("User", login.Roles);
    }

    [Fact]
    public async Task VerificationToken_WorksOnlyOnce()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterAsync(Email);
        var token = host.Email.VerificationTokens[Email];

        Assert.True(await host.RunAsync(auth => auth.VerifyEmailAsync(token)));
        Assert.False(await host.RunAsync(auth => auth.VerifyEmailAsync(token)));
    }

    [Fact]
    public async Task Login_FiveWrongPasswords_LocksTheAccount()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterVerifiedUserAsync(Email);

        for (var i = 0; i < 5; i++)
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.LoginAsync(Email, "Wrong#Pass2026"));

        var locked = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.LoginAsync(Email));
        Assert.Contains("Too many", locked.Message);
    }

    [Fact]
    public async Task Login_UnknownEmail_LooksLikeWrongPassword()
    {
        using var host = await IdentityTestHost.CreateAsync();

        var error = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.LoginAsync("nobody@neuropath.test"));

        Assert.Equal("Invalid credentials.", error.Message);
    }

    [Fact]
    public async Task Refresh_RotatesToken_AndOldTokenStillWorksWithinGracePeriod()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterVerifiedUserAsync(Email);
        var login = await host.LoginAsync(Email);

        var first = await host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null));
        var again = await host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null));

        Assert.NotEqual(login.RefreshToken, first.RefreshToken);
        Assert.NotEqual(first.RefreshToken, again.RefreshToken);
        var next = await host.RunAsync(auth => auth.RefreshAsync(first.RefreshToken!, null));
        Assert.False(string.IsNullOrEmpty(next.AccessToken));
    }

    [Fact]
    public async Task Refresh_ReusedAfterGracePeriod_RevokesEverySessionAndChangesStamp()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterVerifiedUserAsync(Email);
        var login = await host.LoginAsync(Email);
        var rotated = await host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null));

        string stampBefore;
        await using (var db = host.NewContext())
        {
            stampBefore = (await db.Users.SingleAsync(u => u.Email == Email)).SecurityStamp!;
            await db.RefreshTokens.Where(t => t.RevokedAt != null)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow.AddMinutes(-5)));
        }

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null)));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.RunAsync(auth => auth.RefreshAsync(rotated.RefreshToken!, null)));

        await using var check = host.NewContext();
        Assert.NotEqual(stampBefore, (await check.Users.SingleAsync(u => u.Email == Email)).SecurityStamp);
        Assert.False(await check.RefreshTokens.AnyAsync(t => t.RevokedAt == null));
    }

    [Fact]
    public async Task Refresh_ExpiredToken_IsRejected()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterVerifiedUserAsync(Email);
        var login = await host.LoginAsync(Email);
        await using (var db = host.NewContext())
            await db.RefreshTokens.ExecuteUpdateAsync(s => s.SetProperty(t => t.ExpiresAt, DateTime.UtcNow.AddMinutes(-1)));

        var error = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null)));

        Assert.Contains("expired", error.Message);
    }

    [Fact]
    public async Task Refresh_UnknownToken_IsRejected()
    {
        using var host = await IdentityTestHost.CreateAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.RunAsync(auth => auth.RefreshAsync("not-a-real-token", null)));
    }

    [Fact]
    public async Task ResetPassword_WorksOnce_AndRevokesSessions()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterVerifiedUserAsync(Email);
        var login = await host.LoginAsync(Email);

        await host.RunAsync(auth => auth.ForgotPasswordAsync(Email, null));
        var token = host.Email.ResetTokens[Email];
        const string newPassword = "Changed#2026pass";

        Assert.True(await host.RunAsync(auth => auth.ResetPasswordAsync(new ResetPasswordDto { Token = token, NewPassword = newPassword })));
        Assert.False(await host.RunAsync(auth => auth.ResetPasswordAsync(new ResetPasswordDto { Token = token, NewPassword = "Another#2026pass" })));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.LoginAsync(Email));
        Assert.False(string.IsNullOrEmpty((await host.LoginAsync(Email, newPassword)).AccessToken));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null)));
    }

    [Fact]
    public async Task ForgotPassword_UnknownEmail_SendsNothingAndDoesNotThrow()
    {
        using var host = await IdentityTestHost.CreateAsync();

        await host.RunAsync(auth => auth.ForgotPasswordAsync("nobody@neuropath.test", null));

        Assert.Empty(host.Email.ResetTokens);
    }

    [Fact]
    public async Task Logout_RevokesRefreshTokensAndChangesStamp()
    {
        using var host = await IdentityTestHost.CreateAsync();
        await host.RegisterVerifiedUserAsync(Email);
        var login = await host.LoginAsync(Email);
        string stampBefore;
        await using (var db = host.NewContext())
            stampBefore = (await db.Users.SingleAsync(u => u.Email == Email)).SecurityStamp!;

        await host.RunAsync(auth => auth.LogoutAsync(login.UserId!));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => host.RunAsync(auth => auth.RefreshAsync(login.RefreshToken!, null)));
        await using var check = host.NewContext();
        Assert.NotEqual(stampBefore, (await check.Users.SingleAsync(u => u.Email == Email)).SecurityStamp);
    }
}
