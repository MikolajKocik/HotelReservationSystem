namespace HotelReservationSystem.Web.Middleware.MiddlewareExtensions;

public static class ErrorMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
