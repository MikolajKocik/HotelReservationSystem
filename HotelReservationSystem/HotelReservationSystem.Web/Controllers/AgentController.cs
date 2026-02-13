using HotelReservationSystem.MCP.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Web.Controllers;

[Route("Agent")]
public class AgentController : Controller
{
    private readonly IAgentService agentService;

    public AgentController(IAgentService agentService)
    {
        this.agentService = agentService;
    }

    [HttpPost("Ask")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ask([FromBody] AskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Message))
        {
            return Json(new { answer = "Proszę podać wiadomość." });
        }

        string answer = await this.agentService.ProcessMessageAsync(request.Message);

        return Json(new { answer });
    }
}

public class AskRequest
{
    public string Message { get; set; } = string.Empty;
}
