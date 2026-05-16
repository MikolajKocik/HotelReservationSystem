using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Infrastructure.Data.Extensions;
using Microsoft.Extensions.Configuration;

namespace HotelReservationSystem.Infrastructure.Extensions;

/// <summary>
/// Main extension methods for registering Infrastructure layer services
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers all Infrastructure layer services
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextBasedServices(configuration);
        services.AddCQRSServices();

        string redisConnectionString = configuration.GetConnectionString("Redis")!;

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "BookIt_Chat_"; 
        });
        return services;
    }
}