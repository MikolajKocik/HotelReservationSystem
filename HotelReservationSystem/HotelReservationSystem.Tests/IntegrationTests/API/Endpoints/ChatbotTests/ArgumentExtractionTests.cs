using HotelReservationSystem.Tests.IntegrationTests.API.Common.Fixture;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Endpoints.ChatbotTests;

public sealed class ArgumentExtractionTests : ControllerFixture
{
    public ArgumentExtractionTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task ProcessMessage_WhenRoomNumberIsExplicitlyProvided_ShouldExtractCorrectRoomNumberAsToolArgument()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenRoomNumberIsSpelledAsText_ShouldConvertAndExtractAsNumericArgument()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenRequiredArgumentIsMissing_ShouldPromptUserForMissingInformation()
    {
        
    }

    [Fact]
    public async Task ProcessMessage_WhenMessageContainsMultipleRooms_ShouldIsolateCurrentGuestRoomCorrectly()
    {
        
    }
}