using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.UseCases;
using HotelReservationSystem.Application.Interfaces.Auth;
using HotelReservationSystem.Application.UseCases.Auth;

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
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IStripeService, StripeService>();

        services.AddScoped<IAuthorizationService, DefaultAuthorizationService>();
        services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

        return services;
    }
}