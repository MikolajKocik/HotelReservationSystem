using HotelReservationSystem.Workers.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Workers.Configuration;

public static class AppConfig
{
    public static void RegisterHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<StatusReservationJob>();
    }
}