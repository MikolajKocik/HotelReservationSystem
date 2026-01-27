using HotelReservationSystem.Application.CQRS.Payments.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using Microsoft.Extensions.Configuration;
using HotelReservationSystem.Application.CQRS.Abstractions;

namespace HotelReservationSystem.Infrastructure.CQRS.Payments.QueryHandlers;

public class GetPaymentInfoQueryHandler : IQueryHandler<GetPaymentInfoQuery, PaymentInfoDto?>
{
    private readonly ICQRSMediator mediator;
    private readonly IStripeService stripeService;
    private readonly IConfiguration configuration;

    public GetPaymentInfoQueryHandler(ICQRSMediator mediator, IStripeService stripeService, IConfiguration configuration)
    {
        this.mediator = mediator;
        this.stripeService = stripeService;
        this.configuration = configuration;
    }

    public async Task<PaymentInfoDto?> HandleAsync(GetPaymentInfoQuery query, CancellationToken cancellationToken = default)
    {
        ReservationDto? reservation = await this.mediator.SendAsync(new GetReservationByIdQuery(query.ReservationId));
        if (reservation == null)
            return null;

        int nights = (reservation.DepartureDate - reservation.ArrivalDate).Days;
        if (nights <= 0)
            nights = 1;

        decimal totalAmount = reservation.RoomPricePerNight * nights;
        string clientSecret = await this.stripeService.CreatePaymentIntentAsync(totalAmount);
        string publishableKey = Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY")
            ?? this.configuration["Stripe:PublishableKey"] ?? string.Empty;

        return new PaymentInfoDto
        {
            ReservationId = query.ReservationId,
            TotalAmount = totalAmount,
            ClientSecret = clientSecret,
            PublishableKey = publishableKey,
            Currency = "PLN"
        };
    }
}