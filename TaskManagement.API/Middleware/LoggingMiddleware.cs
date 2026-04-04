using System.Diagnostics;
namespace TaskManagement.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString.ToString();
        var traceId = context.TraceIdentifier;

        _logger.LogInformation("Incoming Request: TraceId: {TraceId}, {Method} {Path} {QueryString}", traceId, method, path, queryString);
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while processing {Method} {Path}",
                method, path);

            throw;
        }
        finally
        {

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation(
                "Outgoing Response: TraceId: {TraceId}, {Method} {Path} returned {StatusCode} in {ElapsedMilliseconds} ms",
                   traceId,
                method,
                path,
                statusCode,
                elapsedMilliseconds);
        }

    }
}
