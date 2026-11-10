using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NeuroPath.Core.Common;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Http;

public partial class AuthorizationMatrixTests : IClassFixture<ApiFactory>
{
    private static readonly HashSet<string> MethodsWithBody = new(StringComparer.OrdinalIgnoreCase) { "POST", "PUT", "PATCH" };

    private readonly ApiFactory _factory;

    public AuthorizationMatrixTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public void EveryControllerEndpoint_DeclaresItsAccess()
    {
        var undeclared = Endpoints()
            .Where(e => !e.AllowsAnonymous && !e.RequiresAuthentication)
            .Select(e => e.ToString())
            .ToList();

        Assert.True(undeclared.Count == 0, "Endpoints without [Authorize] or [AllowAnonymous]:\n" + string.Join("\n", undeclared));
    }

    [Fact]
    public void OnlyReadEndpointsAndAuthFlows_AreAnonymous()
    {
        var anonymousWrites = Endpoints()
            .Where(e => e.AllowsAnonymous && e.Method != "GET" && !e.Route.StartsWith("api/auth/", StringComparison.Ordinal))
            .Select(e => e.ToString())
            .ToList();

        Assert.True(anonymousWrites.Count == 0, "Anonymous write endpoints:\n" + string.Join("\n", anonymousWrites));
    }

    [Fact]
    public async Task AnonymousRequests_ToProtectedEndpoints_Get401()
    {
        using var client = _factory.ClientFor(null);
        var failures = new List<string>();

        foreach (var endpoint in Endpoints().Where(e => !e.AllowsAnonymous))
        {
            var status = await SendAsync(client, endpoint);
            if (status != HttpStatusCode.Unauthorized) failures.Add($"{endpoint} -> {(int)status}");
        }

        Assert.True(failures.Count == 0, "Expected 401:\n" + string.Join("\n", failures));
    }

    [Fact]
    public async Task RegularUser_OnRoleRestrictedEndpoints_Gets403()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);
        var failures = new List<string>();

        foreach (var endpoint in Endpoints().Where(e => !e.AllowsAnonymous && !e.Allows(AppRoles.User)))
        {
            var status = await SendAsync(client, endpoint);
            if (status != HttpStatusCode.Forbidden) failures.Add($"{endpoint} -> {(int)status}");
        }

        Assert.True(failures.Count == 0, "Expected 403:\n" + string.Join("\n", failures));
    }

    [Fact]
    public async Task Trainer_IsKeptOutOfAdminEndpoints_ButPassesContentAuthorization()
    {
        var account = await _factory.CreateAccountAsync(AppRoles.Trainer);
        using var client = _factory.ClientFor(account.AccessToken);
        var failures = new List<string>();

        foreach (var endpoint in Endpoints().Where(e => !e.AllowsAnonymous && !e.Allows(AppRoles.Trainer)))
        {
            var status = await SendAsync(client, endpoint);
            if (status != HttpStatusCode.Forbidden) failures.Add($"{endpoint} -> {(int)status}, expected 403");
        }

        var contentCreates = Endpoints().Where(e => e.Method == "POST" && !e.Route.Contains('{') && e.Allows(AppRoles.Trainer) && !e.Allows(AppRoles.User));
        foreach (var endpoint in contentCreates)
        {
            var status = await SendAsync(client, endpoint);
            if (status is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden) failures.Add($"{endpoint} -> {(int)status}, expected to pass authorization");
        }

        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    private static async Task<HttpStatusCode> SendAsync(HttpClient client, EndpointInfo endpoint)
    {
        using var request = new HttpRequestMessage(new HttpMethod(endpoint.Method), endpoint.SampleUrl);
        if (endpoint.AcceptsMultipart)
            request.Content = new MultipartFormDataContent { { new ByteArrayContent(new byte[] { 1 }), "file", "probe.png" } };
        else if (MethodsWithBody.Contains(endpoint.Method))
            request.Content = JsonContent.Create(new { });
        using var response = await client.SendAsync(request);
        return response.StatusCode;
    }

    private List<EndpointInfo> Endpoints()
    {
        _ = _factory.Server;
        return _factory.Services.GetServices<EndpointDataSource>()
            .SelectMany(source => source.Endpoints)
            .OfType<RouteEndpoint>()
            .Where(e => e.Metadata.GetMetadata<ControllerActionDescriptor>() != null)
            .SelectMany(e => (e.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods ?? new[] { "GET" })
                .Select(method => new EndpointInfo(
                    method.ToUpperInvariant(),
                    e.RoutePattern.RawText!.TrimStart('/'),
                    e.Metadata.GetMetadata<IAllowAnonymous>() != null,
                    e.Metadata.GetOrderedMetadata<IAuthorizeData>().ToList(),
                    AcceptsMultipart(e))))
            .DistinctBy(e => (e.Method, e.Route))
            .ToList();
    }

    private static bool AcceptsMultipart(RouteEndpoint endpoint)
        => endpoint.Metadata.GetOrderedMetadata<IAcceptsMetadata>().Any(m => m.ContentTypes.Any(t => t.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase)))
            || endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()!.Parameters.Any(p => p.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile));

    private sealed partial record EndpointInfo(string Method, string Route, bool AllowsAnonymous, IReadOnlyList<IAuthorizeData> Authorization, bool AcceptsMultipart)
    {
        public bool RequiresAuthentication => Authorization.Count > 0;

        public string SampleUrl => "/" + RouteParameter().Replace(Route, "1");

        public bool Allows(string role)
            => Authorization
                .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                .All(a => a.Roles!.Split(',', StringSplitOptions.TrimEntries).Contains(role, StringComparer.OrdinalIgnoreCase));

        public override string ToString() => $"{Method} /{Route}";

        [GeneratedRegex(@"\{[^}]+\}")]
        private static partial Regex RouteParameter();
    }
}
