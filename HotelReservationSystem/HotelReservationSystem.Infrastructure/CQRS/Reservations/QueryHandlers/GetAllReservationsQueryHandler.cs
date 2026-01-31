using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Reservations.Queries;
using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.QueryHandlers;

/// <summary>
/// Handler for retrieving all reservations
/// </summary>
public sealed class GetAllReservationsQueryHandler : IQueryHandler<GetAllReservationsQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationRepository reservationRepository;

    public GetAllReservationsQueryHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the query to get all reservations
    /// </summary>
    public async Task<IEnumerable<ReservationDto>> HandleAsync(GetAllReservationsQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Reservation> reservations = await this.reservationRepository.GetAllAsync();

        return reservations.Select(r => new ReservationDto
        {
            Id = r.Id,
            ArrivalDate = r.ArrivalDate,
            DepartureDate = r.DepartureDate,
            NumberOfGuests = r.NumberOfGuests,
            TotalPrice = r.TotalPrice,
            AdditionalRequests = r.AdditionalRequests ?? string.Empty,
            Status = r.Status,
            Reason = r.Reason ?? string.Empty,
            CreatedAt = r.CreatedAt,
            RoomId = r.RoomId,
            RoomNumber = r.Room != null ? r.Room.Number : string.Empty,
            RoomType = r.Room != null ? r.Room.Type : default,
            RoomPricePerNight = r.Room != null ? r.Room.PricePerNight : 0m,
            GuestId = r.GuestId,
            GuestFirstName = r.Guest != null ? r.Guest.FirstName : string.Empty,
            GuestLastName = r.Guest != null ? r.Guest.LastName : string.Empty,
            GuestEmail = r.Guest != null ? r.Guest.Email : string.Empty,
            GuestPhoneNumber = r.Guest != null ? r.Guest.PhoneNumber : string.Empty,
            PaymentId = r.PaymentId,
            PaymentStatus = r.Payment != null ? r.Payment.Status : null
        }).ToList();
    }
}