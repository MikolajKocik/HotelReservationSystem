using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;

namespace HotelReservationSystem.Application.CQRS.Payments.Queries;

public record GetPaymentInfoQuery(string ReservationId) : IQuery<PaymentInfoDto?>;