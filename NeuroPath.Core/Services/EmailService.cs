using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Settings;

namespace NeuroPath.Core.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtp;
    private readonly ClientAppSettings _client;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> smtp, IOptions<ClientAppSettings> client, ILogger<EmailService> logger)
    {
        _smtp = smtp.Value;
        _client = client.Value;
        _logger = logger;
    }

    public Task SendVerificationEmailAsync(string email, string firstName, string token)
    {
        var url = BuildClientUrl("auth/verify-email", token);
        return SendAsync(email, "Verify your NeuroPath email",
            $"<h2>Hello {Encode(firstName)}!</h2><p>Click the button below to verify your email address.</p>{Button(url, "Verify email", "#667eea")}<p>This link is valid for 24 hours.</p>");
    }

    public Task SendPasswordResetEmailAsync(string email, string firstName, string token)
    {
        var url = BuildClientUrl("auth/reset-password", token);
        return SendAsync(email, "Reset your NeuroPath password",
            $"<h2>Hello {Encode(firstName)}!</h2><p>Click the button below to choose a new password.</p>{Button(url, "Reset password", "#e53e3e")}<p>This link is valid for 1 hour. If you did not request it, you can ignore this email.</p>");
    }

    public Task SendWelcomeEmailAsync(string email, string firstName)
        => SendAsync(email, "Welcome to NeuroPath!",
            $"<h2>Welcome, {Encode(firstName)}!</h2><p>Your email is verified. Start your cognitive journey at NeuroPath!</p>");

    private string BuildClientUrl(string path, string token)
        => $"{_client.BaseUrl.TrimEnd('/')}/{path}?token={Uri.EscapeDataString(token)}";

    private static string Encode(string value) => WebUtility.HtmlEncode(value);

    private static string Button(string url, string text, string color)
        => $"<a href=\"{WebUtility.HtmlEncode(url)}\" style=\"background:{color};color:white;padding:12px 24px;border-radius:8px;text-decoration:none;\">{text}</a>";

    private async Task SendAsync(string to, string subject, string html)
    {
        if (_smtp.LogEmailsInsteadOfSending)
        {
            _logger.LogWarning("Email delivery is disabled. To: {To} | Subject: {Subject} | Body: {Body}", to, subject, html);
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

        try
        {
            using var client = new SmtpClient();
            var socketOptions = _smtp.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
            await client.ConnectAsync(_smtp.Host, _smtp.Port, socketOptions);
            if (!string.IsNullOrEmpty(_smtp.Username))
                await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email \"{Subject}\"", subject);
        }
    }
}
