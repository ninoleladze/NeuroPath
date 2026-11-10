using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NeuroPath.Core.Common;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.Auth;
using NeuroPath.Data.Context;

namespace NeuroPath.Tests.Infrastructure;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public const string AdminEmail = "admin@neuropath.test";
    public const string AdminPassword = "Admin#Test2026";
    public const string UserPassword = "User#Pass2026";

    private readonly SqliteFileDatabase _database = new();

    static ApiFactory()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        Environment.SetEnvironmentVariable("JwtSettings__SecretKey", "api-test-signing-key-0123456789-abcdefghijklmnopqrstuvwxyz");
        Environment.SetEnvironmentVariable("Seed__AdminEmail", AdminEmail);
        Environment.SetEnvironmentVariable("Seed__AdminPassword", AdminPassword);
        Environment.SetEnvironmentVariable("Seed__ApplyMigrations", "false");
        Environment.SetEnvironmentVariable("BackgroundJobs__Enabled", "false");
        Environment.SetEnvironmentVariable("SmtpSettings__LogEmailsInsteadOfSending", "true");
        Environment.SetEnvironmentVariable("RateLimitSettings__PermitLimit", "100000");
        Environment.SetEnvironmentVariable("RateLimitSettings__AuthEndpointPermitLimit", "100000");
        Environment.SetEnvironmentVariable("RateLimitSettings__FileUploadPermitLimit", "100000");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<NeuroPathDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<NeuroPathDbContext>>();
            services.AddDbContext<NeuroPathDbContext>(options => options.UseSqlite(_database.ConnectionString));
            _database.EnsureCreated();
        });
    }

    public HttpClient ClientFor(string? accessToken)
    {
        var client = CreateClient();
        if (accessToken != null)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }

    public async Task<AuthResponseDto> LoginAsync(string email, string password)
    {
        var response = await CreateClient().PostAsJsonAsync("/api/auth/login", new LoginDto { Email = email, Password = password });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AuthResponseDto>())!;
    }

    public Task<AuthResponseDto> LoginAdminAsync() => LoginAsync(AdminEmail, AdminPassword);

    public async Task<TestAccount> CreateAccountAsync(string role = AppRoles.User)
    {
        var name = $"u{Guid.NewGuid():N}"[..13];
        var email = $"{name}@neuropath.test";
        var response = await CreateClient().PostAsJsonAsync("/api/auth/register", new RegisterDto
        {
            UserName = name,
            Email = email,
            Password = UserPassword,
            FirstName = "Test",
            LastName = role
        });
        response.EnsureSuccessStatusCode();

        await using (var scope = Services.CreateAsyncScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var user = (await users.FindByEmailAsync(email))!;
            user.IsEmailVerified = true;
            user.EmailConfirmed = true;
            await users.UpdateAsync(user);
            if (role != AppRoles.User)
            {
                await users.RemoveFromRoleAsync(user, AppRoles.User);
                await users.AddToRoleAsync(user, role);
            }
        }

        var login = await LoginAsync(email, UserPassword);
        return new TestAccount(login.UserId!, email, login.AccessToken!, login.RefreshToken!);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _database.Dispose();
    }
}

public sealed record TestAccount(string Id, string Email, string AccessToken, string RefreshToken);
