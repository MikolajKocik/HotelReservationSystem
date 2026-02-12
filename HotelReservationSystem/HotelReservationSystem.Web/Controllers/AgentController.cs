using HotelReservationSystem.MCP.Server.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace HotelReservationSystem.Web.Controllers;

[Route("Agent")]
public class AgentController : Controller
{
    private readonly ReceptionTools receptionTools;

    public AgentController(ReceptionTools receptionTools)
    {
        this.receptionTools = receptionTools;
    }

    [HttpPost("Ask")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ask([FromBody] AskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Message))
        {
            return Json(new { answer = "Proszê podaæ wiadomoœæ." });
        }

        string message = request.Message.ToLower();
        string answer;

        if (message.Contains("powiadom") || message.Contains("zg³oszenie"))
        {
            // Extract message and category if possible
            string notificationMessage = request.Message;
            string category = "reception";

            if (message.Contains("housekeeping")) category = "housekeeping";
            else if (message.Contains("technical")) category = "technical";

            answer = await this.receptionTools.NotifyStaffAsync(notificationMessage, category);
        }
        else if (message.Contains("rezerwacja") || message.Contains("book"))
        {
            // Parse parameters from message
            var match = Regex.Match(
                request.Message, 
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

        return Json(new { answer });
    }
}

public class AskRequest
{
    public string Message { get; set; } = string.Empty;
}
