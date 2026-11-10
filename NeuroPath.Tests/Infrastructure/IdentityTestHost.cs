using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.Auth;
using NeuroPath.Core.Services;
using NeuroPath.Core.Settings;
using NeuroPath.Data.Context;

namespace NeuroPath.Tests.Infrastructure;

public sealed class IdentityTestHost : IDisposable
{
    public const string Password = "Player#2026pass";

    private readonly SqliteFileDatabase _database = new();
    private readonly ServiceProvider _services;

    private IdentityTestHost()
    {
        _database.EnsureCreated();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<NeuroPathDbContext>(options => options.UseSqlite(_database.ConnectionString));
        services.AddScoped<INeuroPathDbContext>(sp => sp.GetRequiredService<NeuroPathDbContext>());
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Tokens.PasswordResetTokenProvider = TestTokenProvider.Name;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<NeuroPathDbContext>()
            .AddTokenProvider<TestTokenProvider>(TestTokenProvider.Name);
        _services = services.BuildServiceProvider();
    }

    public static JwtSettings Jwt { get; } = new()
    {
        SecretKey = "test-signing-key-0123456789-abcdefghijklmnopqrstuvwxyz",
        Issuer = "NeuroPath.Tests",
        Audience = "NeuroPath.Tests",
        AccessTokenExpirationMinutes = 30,
        RefreshTokenExpirationDays = 7
    };

    public FakeEmailService Email { get; } = new();

    public static async Task<IdentityTestHost> CreateAsync()
    {
        var host = new IdentityTestHost();
        await using var scope = host._services.CreateAsyncScope();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        foreach (var role in AppRoles.All)
            await roles.CreateAsync(new Role { Name = role });
        return host;
    }

    public async Task<T> RunAsync<T>(Func<AuthService, Task<T>> action)
    {
        await using var scope = _services.CreateAsyncScope();
        return await action(CreateAuth(scope.ServiceProvider));
    }

    public async Task RunAsync(Func<AuthService, Task> action)
    {
        await using var scope = _services.CreateAsyncScope();
        await action(CreateAuth(scope.ServiceProvider));
    }

    public NeuroPathDbContext NewContext() => _database.NewContext();

    public async Task<string> RegisterVerifiedUserAsync(string email)
    {
        await RegisterAsync(email);
        await RunAsync(auth => auth.VerifyEmailAsync(Email.VerificationTokens[email]));
        return email;
    }

    public Task<AuthResponseDto> RegisterAsync(string email)
        => RunAsync(auth => auth.RegisterAsync(new RegisterDto
        {
            UserName = email.Split('@')[0],
            Email = email,
            Password = Password,
            FirstName = "Test",
            LastName = "Player"
        }, null));

    public Task<AuthResponseDto> LoginAsync(string email, string? password = null)
        => RunAsync(auth => auth.LoginAsync(new LoginDto { Email = email, Password = password ?? Password }, null));

    public void Dispose()
    {
        _services.Dispose();
        _database.Dispose();
    }

    private AuthService CreateAuth(IServiceProvider provider)
        => new(
            provider.GetRequiredService<UserManager<User>>(),
            Options.Create(Jwt),
            provider.GetRequiredService<INeuroPathDbContext>(),
            Email,
            NullLogger<AuthService>.Instance);
}
