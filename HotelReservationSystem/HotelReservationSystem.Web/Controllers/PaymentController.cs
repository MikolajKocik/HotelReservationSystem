using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Payments.Commands;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Application.CQRS.Payments.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;

namespace HotelReservationSystem.Controllers;

public sealed class PaymentController : Controller
{
    private readonly ICQRSMediator mediator;
    private readonly IStripeService stripeService;

    public PaymentController(ICQRSMediator mediator, IStripeService stripeService)
    {
        this.mediator = mediator;
        this.stripeService = stripeService;
    }

    [HttpGet]
    [Authorize(Policy = "RequireAnyUser")]
    public async Task<IActionResult> Pay(string reservationId)
    {
        var query = new GetPaymentInfoQuery(reservationId);
        PaymentInfoDto? paymentInfo = await this.mediator.SendAsync(query);

        if (paymentInfo == null)
            return NotFound("Reservation not found.");

        if (string.IsNullOrEmpty(paymentInfo.PublishableKey))
        {
            return BadRequest("Stripe Publishable Key is not configured.");
        }

        string successBase = Url.Action("Success", "Payment", new { reservationId = paymentInfo.ReservationId }, Request.Scheme) ?? "/";
        string cancelUrl = Url.Action("Cancel", "Payment", new { reservationId = paymentInfo.ReservationId }, Request.Scheme) ?? "/";
        string successUrl = $"{successBase}?session_id={{CHECKOUT_SESSION_ID}}";

        string sessionUrl = await stripeService.CreateCheckoutSessionAsync(paymentInfo.ReservationId, paymentInfo.TotalAmount, paymentInfo.Currency, successUrl, cancelUrl);

        return Redirect(sessionUrl);
    }

    [HttpGet]
    [Authorize(Policy = "RequireAnyUser")]
    public async Task<IActionResult> Success(string reservationId, string? session_id)
    {
        if (string.IsNullOrWhiteSpace(reservationId) || string.IsNullOrWhiteSpace(session_id))
        {
            return RedirectToAction("MyReservations", "Reservation");
        }

        string? paymentIntentId = await stripeService.GetCheckoutSessionPaymentIntentIdAsync(session_id);
        if (!string.IsNullOrWhiteSpace(paymentIntentId))
        {
            var markCommand = new MarkReservationAsPaidCommand(reservationId, paymentIntentId);
            await this.mediator.SendAsync(markCommand);
        }

        return RedirectToAction("MyReservations", "Reservation");
    }

    [HttpGet]
    [Authorize(Policy = "RequireAnyUser")]
    public IActionResult Cancel(string reservationId)
    {
        return RedirectToAction("MyReservations", "Reservation");
    }

    [HttpPost]
    [Authorize(Policy = "RequireAnyUser")]
    public async Task<IActionResult> ProcessPayment(string reservationId, string paymentMethodId)
    {
        try
        {
            var confirmCommand = new ConfirmPaymentCommand(reservationId, paymentMethodId);
            await this.mediator.SendAsync(confirmCommand);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Policy = "RequireStaff")]
    public async Task<IActionResult> Transactions()
    {
        var query = new GetTransactionsQuery();
        IEnumerable<Payment> transactions = await this.mediator.SendAsync(query)
            ?? Enumerable.Empty<Payment>();

        return View(transactions);
    }
}
