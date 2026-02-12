using HotelReservationSystem.MCP.Server.Tools;
using System.Text.RegularExpressions;

namespace HotelReservationSystem.Web.Services;

public interface IAgentService
{
    Task<string> ProcessMessageAsync(string message);
}

public class AgentService : IAgentService
{
    private readonly ReceptionTools receptionTools;

    public AgentService(ReceptionTools receptionTools)
    {
        this.receptionTools = receptionTools;
    }

    public async Task<string> ProcessMessageAsync(string message)
    {
        string lowerMessage = message.ToLower();
        string answer;

        if (lowerMessage.Contains("powiadom") || lowerMessage.Contains("zg³oszenie"))
        {
            // Extract message and category if possible
            string notificationMessage = message;
            string category = "reception";

            if (lowerMessage.Contains("housekeeping")) category = "housekeeping";
            else if (lowerMessage.Contains("technical")) category = "technical";

            answer = await this.receptionTools.NotifyStaffAsync(notificationMessage, category);
        }
        else if (lowerMessage.Contains("rezerwacja") || lowerMessage.Contains("book"))
        {
            // Parse parameters from message
            var match = Regex.Match(
                message,
                @"(?:rezerwuj|book)\s+pokój\s+(\d+)\s+od\s+(\d{4}-\d{2}-\d{2})\s+do\s+(\d{4}-\d{2}-\d{2})\s+dla\s+(\d+)\s+goœci",
                RegexOptions.IgnoreCase);

            if (match.Success)
            {
                int roomId = int.Parse(match.Groups[1].Value);
                DateTime arrival = DateTime.Parse(match.Groups[2].Value);
                DateTime departure = DateTime.Parse(match.Groups[3].Value);
                int guests = int.Parse(match.Groups[4].Value);

                answer = await this.receptionTools.BookRoomAsync(arrival, departure, roomId, guests);
            }
            else
            {
                answer = "Aby dokonaæ rezerwacji, proszê podaæ w formacie: 'rezerwuj pokój [ID] od [YYYY-MM-DD] do [YYYY-MM-DD] dla [liczba] goœci'";
            }
        }
        else
        {
            answer = "Przepraszam, nie rozumiem. Mogê pomóc z rezerwacj¹ pokoju lub zg³oszeniem do personelu.";
        }

        return answer;
    }
}