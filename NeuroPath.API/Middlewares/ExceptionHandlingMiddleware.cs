using System.Net;
using System.Text.Json;

namespace NeuroPath.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try { await _next(ctx); }
        catch (UnauthorizedAccessException ex) { await Write(ctx, HttpStatusCode.Unauthorized, ex.Message); }
        catch (InvalidOperationException ex) { await Write(ctx, HttpStatusCode.BadRequest, ex.Message); }
        catch (KeyNotFoundException ex) { await Write(ctx, HttpStatusCode.NotFound, ex.Message); }
        catch (Exception ex) { _logger.LogError(ex, "Unhandled exception"); await Write(ctx, HttpStatusCode.InternalServerError, "An unexpected error occurred."); }
    }

    private static async Task Write(HttpContext ctx, HttpStatusCode code, string msg)
    {
        ctx.Response.StatusCode = (int)code;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(new { error = msg }));
    }
}
