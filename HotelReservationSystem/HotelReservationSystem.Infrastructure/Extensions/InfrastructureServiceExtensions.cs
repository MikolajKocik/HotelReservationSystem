using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Infrastructure.Extensions;
using HotelReservationSystem.Infrastructure.Data.Extensions;

namespace HotelReservationSystem.Infrastructure.Extensions;

/// <summary>
/// Main extension methods for registering Infrastructure layer services
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers all Infrastructure layer services
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        services.AddDbContextBasedServices(configuration);
        services.AddCQRSServices();

        return services;
    }
}