using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving a reservation by ID
/// </summary>
public sealed class GetReservationByIdQueryHandler : IQueryHandler<GetReservationByIdQuery, ReservationDto?>
{
    private readonly IReservationRepository reservationRepository;

    public GetReservationByIdQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get a reservation by ID
    /// </summary>
    public async Task<ReservationDto?> HandleAsync(GetReservationByIdQuery query, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await this.reservationRepository.GetByIdAsync(query.Id);

        if (reservation == null)
        {
            return null;
        }

        return new ReservationDto
        {
            Id = reservation.Id,
            ArrivalDate = reservation.ArrivalDate,
            DepartureDate = reservation.DepartureDate,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice,
            AdditionalRequests = reservation.AdditionalRequests,
            Status = reservation.Status,
            Reason = reservation.Reason,
            CreatedAt = reservation.CreatedAt,
            RoomId = reservation.RoomId,
            RoomNumber = reservation.Room.Number,
            RoomType = reservation.Room.Type,
            RoomPricePerNight = reservation.Room.PricePerNight,
            GuestId = reservation.GuestId,
            GuestFirstName = reservation.Guest.FirstName,
            GuestLastName = reservation.Guest.LastName,
            GuestEmail = reservation.Guest.Email,
            PaymentId = reservation.PaymentId,
            PaymentStatus = reservation.Payment != null ? reservation.Payment.Status : null
        };
    }
}