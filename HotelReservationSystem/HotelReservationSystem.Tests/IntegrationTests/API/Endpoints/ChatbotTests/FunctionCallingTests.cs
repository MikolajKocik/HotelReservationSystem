using System.Net.Http.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FluentAssertions;
using HotelReservationSystem.Tests.IntegrationTests.API.Common.Fixture;
using HotelReservationSystem.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Endpoints.ChatbotTests;

public sealed class FunctionCallingTests : ControllerFixture
{
    public FunctionCallingTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ProcessMessage_WhenGuestReportsMissingTowels_ShouldTriggerNotifyStaffTool()
    {
        // Arrange
        string msg = @"Dzień dobry. Zgłaszam brak ręcznika w pokoju numer 7, 
        proszę obsługę o przyniesienie";

        AskRequest request = new()
        {
            Message = msg,
            SessionId = Guid.NewGuid()
        };
    
        QueueServiceClient queueServiceClient = Factory.Services.GetRequiredService<QueueServiceClient>();
        QueueClient queueClient = queueServiceClient.GetQueueClient("hotel-staff-mutations");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        // 2. Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/Agent/Ask", request);

        // 3. Assert
        response.EnsureSuccessStatusCode(); 

        QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
        messages.Should().NotBeEmpty("Ollama powinna zdecydować o powiadomieniu personelu.");
        
        string queueMessageText = messages[0].Body.ToString();
        queueMessageText.Should().Contain("\"RoomNumber\":7");
        queueMessageText.Should().Contain("brak ręcznika");
    }
    
    [Fact]
    public async Task ProcessMessage_WhenGuestAsksGeneralQuestionAboutHotelHours_ShouldReplyDirectlyWithoutCallingTools()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenGuestRequestIsAmbiguous_ShouldAskForClarificationInsteadOfCallingTool()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenRequestIsCompletelyUnrelatedToHotel_ShouldNotTriggerAnyBusinessTools()
    {
        
    }
}