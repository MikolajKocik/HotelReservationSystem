namespace HotelReservationSystem.Web.Middleware.MiddlewareExtensions;

public static class CultureHandlingMiddlewareExtensions
{
    /// <summary>
    /// Adds the CultureHandlingMiddleware to the application's request pipeline.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCultureHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CultureHandlingMiddleware>();
    }
}