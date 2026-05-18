namespace HotelReservationSystem.Application.Interfaces;

public interface INotificationQueueService
{
    Task SendStaffNotificationAsync(int roomId, string message, string category, CancellationToken cancellationToken);
}