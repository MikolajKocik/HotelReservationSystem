using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.UseCases;

namespace HotelReservationSystem.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registers all Application layer services
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IStripeService, StripeService>();

        return services;
    }
}