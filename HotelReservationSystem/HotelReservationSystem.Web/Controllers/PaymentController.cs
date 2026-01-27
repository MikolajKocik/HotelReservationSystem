using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Payments.Commands;
using HotelReservationSystem.Application.Dtos.Reservation;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Application.CQRS.Payments.Queries;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Controllers;

public sealed class PaymentController : Controller
{
    private readonly ICQRSMediator mediator;

    public PaymentController(
        ICQRSMediator mediator
        )
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Pay(string reservationId)
    {
        var query = new GetPaymentInfoQuery(reservationId);
        PaymentInfoDto? paymentInfo = await mediator.SendAsync(query);

        if (paymentInfo == null)
            return NotFound("Reservation not found.");

        var viewModel = new PaymentViewModel
        {
            ReservationId = paymentInfo.ReservationId,
            TotalAmount = paymentInfo.TotalAmount,
            ClientSecret = paymentInfo.ClientSecret,
            PublishableKey = paymentInfo.PublishableKey,
            Currency = paymentInfo.Currency
        };

        if (string.IsNullOrEmpty(viewModel.PublishableKey))
        {
            return BadRequest("Stripe Publishable Key is not configured.");
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment(string reservationId, string paymentMethodId)
    {
        try
        {
            var confirmCommand = new ConfirmPaymentCommand(reservationId, paymentMethodId);
            await mediator.SendAsync(confirmCommand);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }
}
