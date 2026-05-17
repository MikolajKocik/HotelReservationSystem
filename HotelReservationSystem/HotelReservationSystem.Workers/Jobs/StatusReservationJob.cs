using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HotelReservationSystem.Workers.Jobs;

public sealed class StatusReservationJob : BackgroundService
{
    private readonly ILogger<StatusReservationJob> _logger;
    private readonly IServiceProvider _provider;

    public StatusReservationJob(IServiceProvider provider, ILogger<StatusReservationJob> logger)
    {
        _provider = provider;    
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting the reservation worker...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try {
                using IServiceScope scope = _provider.CreateScope();
                IReservationRepository repository = scope.ServiceProvider.GetRequiredService<IReservationRepository>();

                IEnumerable<Reservation> reservationsToClose = await repository.GetExpiredReservations(stoppingToken);
                                
                if (reservationsToClose.Any())
                {
                    IEnumerable<int> ids = reservationsToClose.Select(r => r.RoomId);

                    IRoomRepository rooms = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
                    
                    foreach (int roomId in ids)
                    {
                        await rooms.ToggleAvailabilityAsync(roomId, stoppingToken);   
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Application shut down...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong...");
            }
        }
    }
}