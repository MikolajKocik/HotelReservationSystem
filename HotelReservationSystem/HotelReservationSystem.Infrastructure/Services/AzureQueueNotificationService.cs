using System.Text;
using System.Text.Json;
using Azure.Storage.Queues;
using HotelReservationSystem.Application.Interfaces;

namespace HotelReservationSystem.Infrastructure.Services;

public sealed class AzureQueueNotificationService : INotificationQueueService
{
    private readonly QueueClient _queueClient;

    public AzureQueueNotificationService(QueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public async Task SendStaffNotificationAsync(int roomId, string message, string category, CancellationToken cancellationToken)
    {
        var payload = new
        {
            text = $"[ZGŁOSZENIE] Kategoria: {category.ToUpper()} | Treść: {message} | Numer pokokju: {roomId}"
        };

        string jsonMsg = JsonSerializer.Serialize(payload);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonMsg);
        string base64Message = Convert.ToBase64String(bytes);

        await _queueClient.SendMessageAsync(base64Message, cancellationToken);
    }
}