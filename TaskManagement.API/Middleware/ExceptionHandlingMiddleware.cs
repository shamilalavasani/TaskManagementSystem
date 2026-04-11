using System.Net;
using TaskManagement.API.Common;

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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Message = exception.Message,
            StatusCode = context.Response.StatusCode
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    // private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    // {
    //context.Response.ContentType = "application/json";

    //    var statusCode = exception switch
    //    {
    //        KeyNotFoundException => HttpStatusCode.NotFound,
    //        ArgumentException => HttpStatusCode.BadRequest,
    //        _ => HttpStatusCode.InternalServerError
    //    };

    //    context.Response.StatusCode = (int)statusCode;

    //    var response = new ErrorResponse
    //    {
    //        Message = statusCode == HttpStatusCode.InternalServerError
    //            ? "An unexpected error occurred."
    //            : exception.Message,
    //        StatusCode = context.Response.StatusCode
    //    };

    //    await context.Response.WriteAsJsonAsync(response);
    //}
}