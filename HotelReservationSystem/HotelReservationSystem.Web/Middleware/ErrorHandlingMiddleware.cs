using System.Text.Json;
namespace HotelReservationSystem.Web.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            ArgumentNullException argEx => new
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Invalid input provided.",
                Details = argEx.Message
            },
            UnauthorizedAccessException => new
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Unauthorized access.",
                Details = "You don't have permission to access this resource."
            },
            KeyNotFoundException notFoundEx => new
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Resource not found.",
                Details = notFoundEx.Message
            },
            _ => new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An unexpected error occurred.",
                Details = exception.Message
            }
        };

        context.Response.StatusCode = errorResponse.StatusCode;
        string errorJson = JsonSerializer.Serialize(new 
        { 
            errorResponse.Message,
            errorResponse.Details 
        });

        await context.Response.WriteAsync(errorJson);
    }
}
