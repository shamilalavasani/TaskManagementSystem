using System.Net;
using System.Text.Json;

namespace TaskManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{

    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //var response = new { message = ex.Message };
            //await context.Response.WriteAsJsonAsync(response);
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
        context.Response.StatusCode = (int)statusCode;
        var response = new
        {
            message = exception.Message,
            statusCode = context.Response.StatusCode
        };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
