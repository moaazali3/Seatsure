using System.Text.Json;
using Seatsure.Application.Exceptions;

namespace Seatsure.Middleware;

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
        catch (AppException ex)
        {
            _logger.LogWarning("Handled business exception: {Type} - {Message}", ex.GetType().Name, ex.Message);
            await HandleAppExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled server exception");
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static Task HandleAppExceptionAsync(HttpContext context, AppException ex)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = ex.StatusCode;

        var title = ex switch
        {
            ValidationException => "Bad Request",
            UnauthorizedException => "Unauthorized",
            ForbiddenException => "Forbidden",
            NotFoundException => "Not Found",
            ConflictException => "Conflict",
            _ => "Error"
        };

        var problem = new
        {
            type = $"https://httpstatuses.com/{ex.StatusCode}",
            title,
            status = ex.StatusCode,
            detail = ex.Message,
            instance = context.Request.Path.Value
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    private static Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problem = new
        {
            type = "https://httpstatuses.com/500",
            title = "Internal Server Error",
            status = StatusCodes.Status500InternalServerError,
            detail = "An unexpected error occurred. Please try again later.",
            instance = context.Request.Path.Value
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
