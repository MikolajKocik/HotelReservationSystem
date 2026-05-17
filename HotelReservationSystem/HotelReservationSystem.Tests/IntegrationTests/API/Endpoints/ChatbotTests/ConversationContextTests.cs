using HotelReservationSystem.Tests.IntegrationTests.API.Common.Fixture;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Endpoints.ChatbotTests;

public sealed class ConversationContextTests : ControllerFixture
{
    public ConversationContextTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ProcessMessage_WhenRoomWasProvidedInPreviousTurn_ShouldRememberRoomNumberInCurrentTurn()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenGuestChangesTheirMind_ShouldUpdateArgumentsBasedOnLatestMessage()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenConversationIsTooLong_ShouldTruncateHistoryWithoutLosingCoreContext()
    {
        
    }
}
