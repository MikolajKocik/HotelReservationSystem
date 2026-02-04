using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Reports.Queries;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Web.Utils.ModelMappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Web.Controllers;

[Authorize(Policy = "RequireManager")]
public sealed class ReportsController : Controller
{
    private readonly ICQRSMediator mediator;

    public ReportsController(ICQRSMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> Reports()
    {
        var query = new GenerateReportQuery(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1));
        ReportDto reportData = await mediator.SendAsync(query);

        var model = reportData.ToViewModel();

        return View(model);
    }
}
