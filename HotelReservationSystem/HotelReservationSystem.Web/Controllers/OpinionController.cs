using HotelReservationSystem.Application.Dtos.Opinion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Opinions.Queries;
using HotelReservationSystem.Application.CQRS.Opinions.Commands;

namespace HotelReservationSystem.Controllers;

[Authorize]
public sealed class OpinionController : Controller
{
    private readonly ICQRSMediator mediator;

    public OpinionController(ICQRSMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var query = new GetOpinionsQuery();
        IEnumerable<OpinionDto> opinions = await mediator.SendAsync(query);
        return Ok(opinions);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(string id)
    {
        var query = new GetOpinionByIdQuery(id);
        OpinionDto? opinion = await mediator.SendAsync(query);
        if (opinion == null)
        {
            return NotFound();
        }

        return Ok(opinion);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOpinionDto dto)
    {
        string? userEmail = User.Identity?.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized();
        }

        var command = new CreateOpinionCommand(userEmail, dto);
        string opinionId = await mediator.SendAsync(command);
        return Ok(new { opinionId });
    }

    [HttpPut]
    public async Task<IActionResult> Edit(UpdateOpinionDto dto)
    {
        string? userEmail = User.Identity?.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized();
        }

        var command = new UpdateOpinionCommand(userEmail, dto);
        await mediator.SendAsync(command);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        string? userEmail = User.Identity?.Name;
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized();
        }

        var command = new DeleteOpinionCommand(userEmail, id);
        await mediator.SendAsync(command);
        return Ok();
    }
}