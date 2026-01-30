using HotelReservationSystem.Application.CQRS.Payments.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Core.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using HotelReservationSystem.Application.CQRS.Abstractions;

namespace HotelReservationSystem.Infrastructure.CQRS.Payments.QueryHandlers;

public sealed class GetPaymentInfoQueryHandler : IQueryHandler<GetPaymentInfoQuery, PaymentInfoDto?>
{
    private readonly ICQRSMediator mediator;
    private readonly IConfiguration configuration;

    public GetPaymentInfoQueryHandler(ICQRSMediator mediator, IConfiguration configuration)
    {
        this.mediator = mediator;
        this.configuration = configuration;
    }

    public async Task<PaymentInfoDto?> HandleAsync(GetPaymentInfoQuery query, CancellationToken cancellationToken = default)
    {
        ReservationDto? reservation = await this.mediator.SendAsync(new GetReservationByIdQuery(query.ReservationId));
        if (reservation == null)
            return null;

        int nights = (reservation.DepartureDate - reservation.ArrivalDate).Days;
        if (nights <= 0)
            throw new ValidationDomainException("Invalid reservation dates: departure must be after arrival");

        decimal totalAmount = reservation.RoomPricePerNight * nights;
        string publishableKey = Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY")
            ?? this.configuration["Stripe:PublishableKey"] ?? string.Empty;

        string clientSecret = string.Empty;

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
