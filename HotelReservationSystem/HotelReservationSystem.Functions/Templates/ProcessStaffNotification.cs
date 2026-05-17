using System.Net.Http.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotelReservationSystem.Functions.Templates;

public sealed class ProcessStaffNotification
{
    private readonly ILogger<ProcessStaffNotification> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _cfg;

    public ProcessStaffNotification(ILogger<ProcessStaffNotification> logger, HttpClient httpClient, IConfiguration cfg)
    {
        _logger = logger;
        _httpClient = httpClient;
        _cfg = cfg;
    }

    [Function(nameof(ProcessStaffNotification))]
    public async Task Run([QueueTrigger("myqueue-items", Connection = "")] QueueMessage message)
    {
        _logger.LogInformation("Queue trigger function processed: {messageText}", message.MessageText);
    
        var discordPayload = new 
        { 
            content = $"|{Guid.NewGuid()}| Aurora notification: {message}" 
        };
        
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_cfg["DiscordWebhook"], discordPayload);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Notification send successful");
        }
        else
        {
            _logger.LogError("Error ocurred while processing message to Discord server: {StatusCode}", response.StatusCode);
        }
    }
}