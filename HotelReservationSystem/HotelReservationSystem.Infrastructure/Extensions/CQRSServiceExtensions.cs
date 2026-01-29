using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Infrastructure.CQRS;
using HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Guests.CommandHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Reports.QueryHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Payments.CommandHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Rooms.CommandHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Rooms.QueryHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;
using HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

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

        services.AddScoped<GetAvailableRoomsQueryHandler>();
        services.AddScoped<GetAllRoomsQueryHandler>();
        services.AddScoped<GetRoomByIdQueryHandler>();
        services.AddScoped<GetAvailableRoomsSelectListQueryHandler>();
        services.AddScoped<CreateRoomCommandHandler>();
        services.AddScoped<UpdateRoomCommandHandler>();
        services.AddScoped<DeleteRoomCommandHandler>();
        services.AddScoped<ToggleRoomAvailabilityCommandHandler>();

        services.AddScoped<GenerateReportQueryHandler>();

        services.AddScoped<ConfirmPaymentCommandHandler>();

        services.AddScoped<GetAllReservationsQueryHandler>();
        services.AddScoped<GetReservationByIdQueryHandler>();
        services.AddScoped<GetReservationsByGuestEmailQueryHandler>();
        services.AddScoped<GetReservationsByDateRangeQueryHandler>();
        services.AddScoped<CreateReservationCommandHandler>();
        services.AddScoped<ConfirmReservationCommandHandler>();
        services.AddScoped<CancelReservationCommandHandler>();
        services.AddScoped<MarkReservationAsPaidCommandHandler>();
        services.AddScoped<UpdateReservationCommandHandler>();

        return services;
    }
}