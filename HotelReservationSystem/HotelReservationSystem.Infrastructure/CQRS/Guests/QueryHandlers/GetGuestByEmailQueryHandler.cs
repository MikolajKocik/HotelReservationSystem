using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;

/// <summary>
/// Handler for retrieving a guest by email
/// </summary>
public class GetGuestByEmailQueryHandler : IQueryHandler<GetGuestByEmailQuery, GuestDto?>
{
    private readonly IGuestRepository guestRepository;

    public GetGuestByEmailQueryHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the query to get a guest by email
    /// </summary>
    public async Task<GuestDto?> HandleAsync(GetGuestByEmailQuery query, CancellationToken cancellationToken = default)
    {
        Guest? guest = await guestRepository.GetByEmailAsync(query.Email);
        
        return guest == null ? null : new GuestDto
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            PhoneNumber = guest.PhoneNumber,
            CreatedAt = guest.CreatedAt,
            ReservationsCount = guest.Reservations.Count
        };
    }
}
