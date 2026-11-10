namespace NeuroPath.API.Middlewares;

public sealed class SecurityHeadersMiddleware
{
    private const string ApiContentSecurityPolicy = "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; form-action 'none'";

    private readonly RequestDelegate _next;
    private readonly PathString _uploadsPath;

    public SecurityHeadersMiddleware(RequestDelegate next, PathString uploadsPath)
    {
        _next = next;
        _uploadsPath = uploadsPath;
    }

    public Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;
            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "DENY";
            headers["Referrer-Policy"] = "no-referrer";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";
            headers["Cross-Origin-Opener-Policy"] = "same-origin";
            headers.Remove("X-Powered-By");

            var path = context.Request.Path;
            if (!path.StartsWithSegments("/swagger") && !path.StartsWithSegments(_uploadsPath))
                headers["Content-Security-Policy"] = ApiContentSecurityPolicy;

            if (path.StartsWithSegments("/api"))
                headers["Cache-Control"] = "no-store";

            return Task.CompletedTask;
        });

        return _next(context);
    }
}
