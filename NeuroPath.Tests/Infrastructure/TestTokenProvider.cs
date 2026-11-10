using Microsoft.AspNetCore.Identity;
using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Tests.Infrastructure;

public sealed class TestTokenProvider : IUserTwoFactorTokenProvider<User>
{
    public const string Name = "Test";

    public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
        => Task.FromResult(Expected(purpose, user));

    public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
        => Task.FromResult(token == Expected(purpose, user));

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user) => Task.FromResult(false);

    private static string Expected(string purpose, User user) => $"{purpose}:{user.Id}:{user.SecurityStamp}";
}
