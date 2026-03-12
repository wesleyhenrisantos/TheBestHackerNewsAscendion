using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TheBestHackerNews.Api.Services;

/// <summary>
/// Handles unhandled exceptions and converts them into RFC 7807 problem details.
/// </summary>
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request.");

        var statusCode = exception switch
        {
            ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
            HttpRequestException => StatusCodes.Status503ServiceUnavailable,
            TaskCanceledException => StatusCodes.Status504GatewayTimeout,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Title = "Request processing failed.",
            Detail = exception.Message,
            Status = statusCode,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
