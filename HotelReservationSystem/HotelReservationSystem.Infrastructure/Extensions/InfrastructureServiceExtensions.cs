using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Infrastructure.Data.Extensions;
using Microsoft.Extensions.Configuration;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Infrastructure.Services;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;

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

        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddQueueServiceClient(configuration.GetConnectionString("AzureStorage"));
        });

        services.AddScoped<INotificationQueueService, AzureQueueNotificationService>(provider =>
        {
            QueueServiceClient queueServiceClient = provider.GetRequiredService<QueueServiceClient>();
           
            QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
            queueClient.CreateIfNotExists(); 
            
            return new AzureQueueNotificationService(queueClient);
        });

        return services;
    }
}