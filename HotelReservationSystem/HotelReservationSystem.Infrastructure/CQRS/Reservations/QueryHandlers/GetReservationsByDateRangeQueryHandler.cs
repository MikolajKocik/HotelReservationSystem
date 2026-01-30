using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving reservations by date range
/// </summary>
public sealed class GetReservationsByDateRangeQueryHandler : IQueryHandler<GetReservationsByDateRangeQuery, IQueryable<ReservationDto>>
{
    private readonly IReservationRepository reservationRepository;

    public GetReservationsByDateRangeQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get reservations by date range
    /// </summary>
    public async Task<IQueryable<ReservationDto>> HandleAsync(GetReservationsByDateRangeQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Reservation> reservations = await this.reservationRepository.GetByDateRangeAsync(query.FromDate, query.ToDate);

        return reservations.Select(r => new ReservationDto
        {
            Id = r.Id,
            ArrivalDate = r.ArrivalDate,
            DepartureDate = r.DepartureDate,
            NumberOfGuests = r.NumberOfGuests,
            TotalPrice = r.TotalPrice,
            AdditionalRequests = r.AdditionalRequests,
            Status = r.Status,
            Reason = r.Reason,
            CreatedAt = r.CreatedAt,
            RoomId = r.RoomId,
            RoomNumber = r.Room.Number,
            RoomType = r.Room.Type,
            RoomPricePerNight = r.Room.PricePerNight,
            GuestId = r.GuestId,
            GuestFirstName = r.Guest.FirstName,
            GuestLastName = r.Guest.LastName,
            GuestEmail = r.Guest.Email,
            PaymentId = r.PaymentId,
            PaymentStatus = r.Payment != null ? r.Payment.Status : null
        });
    }
}