using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using System.Linq;
using System.Reflection;
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

        Assembly handlersAssembly = typeof(GetAllGuestsQueryHandler).Assembly;

        IEnumerable<Type> handlerTypes = handlersAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract);

        foreach (Type implType in handlerTypes)
        {
            IEnumerable<Type> interfaces = implType.GetInterfaces().Where(i => i.IsGenericType);

            foreach (Type iface in interfaces)
            {
                Type def = iface.GetGenericTypeDefinition();

                if (def == typeof(IQueryHandler<,>) || def == typeof(ICommandHandler<>) || def == typeof(ICommandHandler<,>))
                {
                    services.AddScoped(iface, implType);
                }
            }
        }

        return services;
    }
}