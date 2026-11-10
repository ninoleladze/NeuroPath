using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;

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

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            if (!context.Response.HasStarted)
                context.Response.StatusCode = 499;
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted) throw;
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        ProblemDetails problem = exception switch
        {
            ValidationException validation => new ValidationProblemDetails(
                validation.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            },
            NotFoundException => Problem(StatusCodes.Status404NotFound, "Not found", exception.Message),
            BusinessRuleException => Problem(StatusCodes.Status400BadRequest, "Request could not be processed", exception.Message),
            ForbiddenException => Problem(StatusCodes.Status403Forbidden, "Forbidden", exception.Message),
            ConflictException => Problem(StatusCodes.Status409Conflict, "Conflict", exception.Message),
            UnauthorizedAccessException => Problem(StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
            DbUpdateConcurrencyException => Problem(StatusCodes.Status409Conflict, "Conflict", "The data was changed by another request. Please retry."),
            DbUpdateException db when IsConstraintViolation(db) => Problem(StatusCodes.Status409Conflict, "Conflict", "The request conflicts with existing data."),
            DbUpdateException db when IsValueTooLong(db) => Problem(StatusCodes.Status400BadRequest, "Request could not be processed", "One of the values is longer than allowed."),
            _ => Problem(StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.")
        };

        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        problem.Extensions["traceId"] = traceId;

        if (problem.Status >= StatusCodes.Status500InternalServerError || exception is DbUpdateException)
            _logger.LogError(exception, "Request {Method} {Path} failed. TraceId: {TraceId}", context.Request.Method, context.Request.Path, traceId);
        else
            _logger.LogInformation("Request {Method} {Path} rejected with {Status}: {Message}", context.Request.Method, context.Request.Path, problem.Status, exception.Message);

        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problem, problem.GetType(), options: null, contentType: "application/problem+json");
    }

    private static readonly int[] ConstraintErrorNumbers = { 547, 2601, 2627 };
    private static readonly int[] TruncationErrorNumbers = { 2628, 8152 };

    private static bool IsConstraintViolation(DbUpdateException exception)
        => exception.InnerException is SqlException sql && ConstraintErrorNumbers.Contains(sql.Number);

    private static bool IsValueTooLong(DbUpdateException exception)
        => exception.InnerException is SqlException sql && TruncationErrorNumbers.Contains(sql.Number);

    private static ProblemDetails Problem(int status, string title, string detail)
        => new() { Status = status, Title = title, Detail = detail };
}
