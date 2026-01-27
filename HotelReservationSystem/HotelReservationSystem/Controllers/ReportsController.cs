using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Application.CQRS.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Kierownik")]
    public class ReportsController : Controller
    {
        private readonly ICQRSMediator mediator;

        public ReportsController(ICQRSMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult Reports()
        {
            // TODO: Create GenerateReportQuery in CQRS structure
            // var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
            // var reportData = await mediator.SendAsync(query);

            var model = new ReportViewModel
            {
                // TODO: Replace with actual CQRS query results
                TotalReservations = 0, // reportData.TotalReservations,
                ConfirmedReservations = 0, // reportData.ConfirmedReservations,
                CanceledReservations = 0, // reportData.CancelledReservations,
                TotalPayments = 0, // reportData.TotalRevenue,
                AvailableRooms = 0 // reportData.AvailableRooms
            };

            return View(model);
        }
    }
}