using System.Collections.Concurrent;
using NeuroPath.Core.Interfaces;

namespace NeuroPath.Tests.Infrastructure;

public sealed class FakeEmailService : IEmailService
{
    public ConcurrentDictionary<string, string> VerificationTokens { get; } = new(StringComparer.OrdinalIgnoreCase);
    public ConcurrentDictionary<string, string> ResetTokens { get; } = new(StringComparer.OrdinalIgnoreCase);

    public Task SendVerificationEmailAsync(string email, string firstName, string token)
    {
        VerificationTokens[email] = token;
        return Task.CompletedTask;
    }

    public Task SendPasswordResetEmailAsync(string email, string firstName, string token)
    {
        ResetTokens[email] = token;
        return Task.CompletedTask;
    }

    public Task SendWelcomeEmailAsync(string email, string firstName) => Task.CompletedTask;
}
