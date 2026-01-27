using Microsoft.Extensions.Primitives;
using System.Globalization;
namespace HotelReservationSystem.Web.Middleware;

public class CultureHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public CultureHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        StringValues cultureQuery = context.Request.Query["culture"];
        string cultureName;

        if (!string.IsNullOrWhiteSpace(cultureQuery))
        {
            cultureName = cultureQuery.ToString();
        }
        else
        {
            cultureName = context.Request.Headers["Accept-Language"]
                .ToString()
                .Split(',')
                .FirstOrDefault() ?? "en-US";
        }

        var culture = new CultureInfo(cultureName);
            
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        await _next(context);
    }
}
