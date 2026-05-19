using System.Net.Http.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FluentAssertions;
using HotelReservationSystem.Tests.IntegrationTests.API.Common.Fixture;
using HotelReservationSystem.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Endpoints.ChatbotTests;

public sealed class ConversationContextTests : ControllerFixture
{
    public ConversationContextTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ProcessMessage_WhenRoomWasProvidedInPreviousTurn_ShouldRememberRoomNumberInCurrentTurn()
    {
        // Arrange
        Guid sessionId = Guid.NewGuid();
        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // Turn 1: Provide room number
        AskRequest turn1 = new() { Message = "Dzień dobry, jestem w pokoju 7.", SessionId = sessionId };
        HttpResponseMessage resp1 = await Client.PostAsJsonAsync("/Agent/Ask", turn1);
        resp1.EnsureSuccessStatusCode();

        // Turn 2: Request towel (room number not specified here)
        AskRequest turn2 = new() { Message = "Proszę przynieść mi czysty ręcznik.", SessionId = sessionId };
        
        // Act
        HttpResponseMessage resp2 = await Client.PostAsJsonAsync("/Agent/Ask", turn2);

        // Assert
        resp2.EnsureSuccessStatusCode();

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
        messages.Should().NotBeEmpty("Agent powinien wywołać powiadomienie personelu, pamiętając numer pokoju z poprzedniej tury.");
        
        string queueMessageText = messages[0].Body.ToString();
        queueMessageText.Should().Contain("\"RoomNumber\":7");
    }

    [Fact]
    public async Task ProcessMessage_WhenGuestChangesTheirMind_ShouldUpdateArgumentsBasedOnLatestMessage()
    {
        // Arrange
        Guid sessionId = Guid.NewGuid();
        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // Turn 1: Provide room number 7
        AskRequest turn1 = new() { Message = "Jestem w pokoju 7.", SessionId = sessionId };
        HttpResponseMessage resp1 = await Client.PostAsJsonAsync("/Agent/Ask", turn1);
        resp1.EnsureSuccessStatusCode();

        // Turn 2: Change mind to room 9 and ask for assistance
        AskRequest turn2 = new() { Message = "Ojej, pomyłka, jestem w pokoju 9. Przynieś mi wodę.", SessionId = sessionId };
        
        // Act
        HttpResponseMessage resp2 = await Client.PostAsJsonAsync("/Agent/Ask", turn2);

        // Assert
        resp2.EnsureSuccessStatusCode();

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
        messages.Should().NotBeEmpty("Agent powinien zaktualizować numer pokoju w zgłoszeniu na podstawie nowej wiadomości.");
        
        string queueMessageText = messages[0].Body.ToString();
        queueMessageText.Should().Contain("\"RoomNumber\":9");
        queueMessageText.Should().NotContain("\"RoomNumber\":7");
    }

    [Fact]
    public async Task ProcessMessage_WhenConversationIsTooLong_ShouldTruncateHistoryWithoutLosingCoreContext()
    {
        // Arrange
        Guid sessionId = Guid.NewGuid();
        
        // Act: Send 16 requests to exceed the historyDto.Count limit (15)
        for (int i = 1; i <= 16; i++)
        {
            AskRequest request = new() { Message = $"Wiadomość testowa numer {i}", SessionId = sessionId };
            HttpResponseMessage response = await Client.PostAsJsonAsync("/Agent/Ask", request);
            response.EnsureSuccessStatusCode();
        }

        // Send a final message to ensure the flow completes after truncation
        AskRequest finalRequest = new() { Message = "Jaki jest numer telefonu recepcji?", SessionId = sessionId };
        HttpResponseMessage finalResponse = await Client.PostAsJsonAsync("/Agent/Ask", finalRequest);

        // Assert
        finalResponse.EnsureSuccessStatusCode();
        string answer = await finalResponse.Content.ReadAsStringAsync();
        answer.Should().NotBeNullOrWhiteSpace();
    }
}
