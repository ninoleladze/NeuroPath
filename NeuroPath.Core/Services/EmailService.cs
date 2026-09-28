using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Settings;

namespace NeuroPath.Core.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtp;
    public EmailService(IOptions<SmtpSettings> smtp) => _smtp = smtp.Value;

    public async Task SendVerificationEmailAsync(string email, string firstName, string token)
    {
        var url = $"http://localhost:4200/auth/verify-email?token={Uri.EscapeDataString(token)}";
        await Send(email, "Verify Your NeuroPath Email",
            $"<h2>Hello {firstName}!</h2><p>Click to verify your email:</p><a href='{url}' style='background:#667eea;color:white;padding:12px 24px;border-radius:8px;text-decoration:none;'>Verify Email</a><p>Link valid for 24 hours.</p>");
    }

    public async Task SendPasswordResetEmailAsync(string email, string firstName, string token)
    {
        var url = $"http://localhost:4200/auth/reset-password?token={Uri.EscapeDataString(token)}";
        await Send(email, "Reset Your NeuroPath Password",
            $"<h2>Hello {firstName}!</h2><p>Click to reset your password:</p><a href='{url}' style='background:#e53e3e;color:white;padding:12px 24px;border-radius:8px;text-decoration:none;'>Reset Password</a><p>Link valid for 1 hour.</p>");
    }

    public async Task SendWelcomeEmailAsync(string email, string firstName)
        => await Send(email, "Welcome to NeuroPath!",
            $"<h2>Welcome, {firstName}!</h2><p>Your email is verified. Start your cognitive journey at NeuroPath!</p>");

    private async Task Send(string to, string subject, string html)
    {
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
        catch { /* Log in production */ }
    }
}
