using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Reports.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Application.Dtos.Report;

namespace HotelReservationSystem.Controllers;

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
        var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
        ReportDto reportData = await mediator.SendAsync(query);

        var model = new ReportViewModel
        {
            TotalReservations = reportData.TotalReservations,
            ConfirmedReservations = reportData.ConfirmedReservations,
            CanceledReservations = reportData.CanceledReservations,
            TotalPayments = reportData.TotalPayments,
            AvailableRooms = reportData.AvailableRooms
        };

        return View(model);
    }
}
