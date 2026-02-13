namespace HotelReservationSystem.MCP.Server.Interfaces;

public interface IAgentService
{
    Task<string> ProcessMessageAsync(string message);
}
