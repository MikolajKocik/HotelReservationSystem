using System.Net.Http.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FluentAssertions;
using HotelReservationSystem.Tests.IntegrationTests.API.Common.Fixture;
using HotelReservationSystem.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Endpoints.ChatbotTests;

public sealed class ArgumentExtractionTests : ControllerFixture
{
    public ArgumentExtractionTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ProcessMessage_WhenRoomNumberIsExplicitlyProvided_ShouldExtractCorrectRoomNumberAsToolArgument()
    {
        // Arrange
        string msg = "Zgłaszam brak papieru toaletowego w pokoju 5";
        AskRequest request = new()
        {
            Message = msg,
            SessionId = Guid.NewGuid()
        };

        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/Agent/Ask", request);

        // Assert
        response.EnsureSuccessStatusCode();

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
        messages.Should().NotBeEmpty("Agent powinien wywołać zgłoszenie dla pokoju 5.");
        
        string queueMessageText = messages[0].Body.ToString();
        queueMessageText.Should().Contain("\"RoomNumber\":5");
    }

    [Fact]
    public async Task ProcessMessage_WhenRoomNumberIsSpelledAsText_ShouldConvertAndExtractAsNumericArgument()
    {
        // Arrange
        string msg = "Zgłaszam niedziałający telewizor w pokoju piątym";
        AskRequest request = new()
        {
            Message = msg,
            SessionId = Guid.NewGuid()
        };

        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/Agent/Ask", request);

        // Assert
        response.EnsureSuccessStatusCode();

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
        messages.Should().NotBeEmpty("Agent powinien rozpoznać słowny zapis numeru pokoju.");
        
        string queueMessageText = messages[0].Body.ToString();
        queueMessageText.Should().Contain("\"RoomNumber\":5");
    }

    [Fact]
    public async Task ProcessMessage_WhenRequiredArgumentIsMissing_ShouldPromptUserForMissingInformation()
    {
        // Arrange
        string msg = "Skończyły się mydła, proszę przynieść";
        AskRequest request = new()
        {
            Message = msg,
            SessionId = Guid.NewGuid()
        };

        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/Agent/Ask", request);

        // Assert
        response.EnsureSuccessStatusCode();

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync();
        messages.Should().BeEmpty("Agent nie może stworzyć zgłoszenia bez podanego numeru pokoju.");

        string content = await response.Content.ReadAsStringAsync();
        content.ToLower().Should().ContainAny(new[] { "pokój", "pokoju", "numer" }, 
            "Agent powinien dopytać o brakujące informacje.");
    }

    [Fact]
    public async Task ProcessMessage_WhenMessageContainsMultipleRooms_ShouldIsolateCurrentGuestRoomCorrectly()
    {
        // Arrange
        string msg = "Kolega z pokoju 10 pożyczył mi ładowarkę, ale w moim pokoju 12 nie działa klimatyzacja";
        AskRequest request = new()
        {
            Message = msg,
            SessionId = Guid.NewGuid()
        };

        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/Agent/Ask", request);

        // Assert
        response.EnsureSuccessStatusCode();

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
        messages.Should().NotBeEmpty("Agent powinien stworzyć zgłoszenie dla pokoju 12.");
        
        string queueMessageText = messages[0].Body.ToString();
        queueMessageText.Should().Contain("\"RoomNumber\":12");
        queueMessageText.Should().NotContain("\"RoomNumber\":10");
    }
}