using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Http;

public class ApiBehaviorTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public ApiBehaviorTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Logout_InvalidatesTheAccessTokenImmediately()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);

        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/users/me")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsync("/api/auth/logout", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/users/me")).StatusCode);

        var refresh = await _factory.CreateClient().PostAsJsonAsync("/api/auth/refresh", new { refreshToken = account.RefreshToken });
        Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);
    }

    [Fact]
    public async Task BlockingAUser_StopsTheirTokenAndLogin()
    {
        var account = await _factory.CreateAccountAsync();
        var admin = await _factory.LoginAdminAsync();
        using var adminClient = _factory.ClientFor(admin.AccessToken);
        using var userClient = _factory.ClientFor(account.AccessToken);

        var block = await adminClient.PutAsJsonAsync($"/api/users/{account.Id}/blocked", new { isBlocked = true });
        Assert.Equal(HttpStatusCode.NoContent, block.StatusCode);

        Assert.Equal(HttpStatusCode.Unauthorized, (await userClient.GetAsync("/api/users/me")).StatusCode);
        var login = await _factory.CreateClient().PostAsJsonAsync("/api/auth/login", new { email = account.Email, password = ApiFactory.UserPassword });
        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);
    }

    [Fact]
    public async Task AdminCannotBlockThemselves()
    {
        var admin = await _factory.LoginAdminAsync();
        using var client = _factory.ClientFor(admin.AccessToken);

        var response = await client.PutAsJsonAsync($"/api/users/{admin.UserId}/blocked", new { isBlocked = true });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BusinessErrors_AreProblemDetailsWithTraceId()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);

        var response = await client.PostAsJsonAsync("/api/bookmarks", new { bookmarkType = "Quiz", referenceId = 99999 });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(404, body.RootElement.GetProperty("status").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(body.RootElement.GetProperty("detail").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(body.RootElement.GetProperty("traceId").GetString()));
    }

    [Fact]
    public async Task ValidationErrors_ListEveryInvalidField()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/auth/register", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var errors = body.RootElement.GetProperty("errors");
        foreach (var field in new[] { "UserName", "Email", "Password", "FirstName", "LastName" })
            Assert.True(errors.TryGetProperty(field, out _), $"Missing validation error for {field}");
    }

    [Fact]
    public async Task Dates_AreSerializedAsUtc()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);

        using var body = JsonDocument.Parse(await client.GetStringAsync("/api/users/me"));

        Assert.EndsWith("Z", body.RootElement.GetProperty("createdAt").GetString());
    }

    [Fact]
    public async Task Settings_AcceptKnownTimeZoneAndRejectUnknown()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);
        var settings = (await client.GetFromJsonAsync<Dictionary<string, object>>("/api/users/me/settings"))!;

        settings["timeZoneId"] = "Mars/Olympus";
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PutAsJsonAsync("/api/users/me/settings", settings)).StatusCode);

        settings["timeZoneId"] = "Asia/Tbilisi";
        Assert.Equal(HttpStatusCode.NoContent, (await client.PutAsJsonAsync("/api/users/me/settings", settings)).StatusCode);

        using var saved = JsonDocument.Parse(await client.GetStringAsync("/api/users/me/settings"));
        Assert.Equal("Asia/Tbilisi", saved.RootElement.GetProperty("timeZoneId").GetString());
    }

    [Fact]
    public async Task Health_IsAnonymous()
    {
        var response = await _factory.CreateClient().GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Readiness_ChecksTheDatabase()
    {
        var response = await _factory.CreateClient().GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData("/api/exercises", HttpStatusCode.OK)]
    [InlineData("/api/users/me", HttpStatusCode.Unauthorized)]
    [InlineData("/api/exercises/999999", HttpStatusCode.NotFound)]
    public async Task Responses_CarrySecurityHeaders(string url, HttpStatusCode expectedStatus)
    {
        var response = await _factory.CreateClient().GetAsync(url);

        Assert.Equal(expectedStatus, response.StatusCode);
        Assert.Equal("nosniff", Header(response, "X-Content-Type-Options"));
        Assert.Equal("DENY", Header(response, "X-Frame-Options"));
        Assert.Equal("no-referrer", Header(response, "Referrer-Policy"));
        Assert.Contains("frame-ancestors 'none'", Header(response, "Content-Security-Policy"));
        Assert.Contains("no-store", response.Headers.CacheControl?.ToString());
        Assert.False(response.Headers.Contains("Server"));
    }

    private static string? Header(HttpResponseMessage response, string name)
        => response.Headers.TryGetValues(name, out var values) || response.Content.Headers.TryGetValues(name, out values)
            ? string.Join(",", values)
            : null;
}
