using HotelReservationSystem.MCP.Server.Interfaces;
using HotelReservationSystem.Web.Filters;
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
    [ServiceFilter(typeof(PromptInjectionFilter))]
    public async Task<IActionResult> Ask([FromBody] AskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Message))
        {
            return Json(new { answer = "Proszę podać wiadomość." });
        }

        Guid sessionId = request.SessionId;

        string answer = await this.agentService.ProcessMessageAsync(sessionId, request.Message, cancellationToken);

        return Json(new { answer });
    }
}

public sealed class AskRequest
{
    public string Message { get; init; } = string.Empty;
    public Guid SessionId { get; init; } = Guid.NewGuid();
}
