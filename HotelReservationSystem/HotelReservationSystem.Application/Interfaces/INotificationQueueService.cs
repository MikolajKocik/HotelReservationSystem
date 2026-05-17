namespace HotelReservationSystem.Application.Interfaces;

public interface INotificationQueueService
{
    Task SendStaffNotificationAsync(string message, string category, CancellationToken cancellationToken);
}