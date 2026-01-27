using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Infrastructure.CQRS;
using HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Guests.CommandHandlers;

namespace HotelReservationSystem.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering CQRS services
/// </summary>
public static class CQRSServiceExtensions
{
    /// <summary>
    /// Registers all CQRS services including mediator and handlers
    /// </summary>
    public static IServiceCollection AddCQRSServices(this IServiceCollection services)
    {
        services.AddScoped<ICQRSMediator, CQRSMediator>();

        services.AddScoped<GetAllGuestsQueryHandler>();
        services.AddScoped<GetGuestByIdQueryHandler>();
        services.AddScoped<GetGuestByEmailQueryHandler>();
        services.AddScoped<CreateGuestCommandHandler>();
        services.AddScoped<UpdateGuestCommandHandler>();
        services.AddScoped<DeleteGuestCommandHandler>();

        return services;
    }
}