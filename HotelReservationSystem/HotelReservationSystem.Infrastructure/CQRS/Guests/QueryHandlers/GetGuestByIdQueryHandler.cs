using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;

/// <summary>
/// Handler for retrieving a guest by ID
/// </summary>
public sealed class GetGuestByIdQueryHandler : IQueryHandler<GetGuestByIdQuery, GuestDto?>
{
    private readonly IGuestRepository guestRepository;

    public GetGuestByIdQueryHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the query to get a guest by ID
    /// </summary>
    public async Task<GuestDto?> HandleAsync(GetGuestByIdQuery query, CancellationToken cancellationToken = default)
    {
        Guest? guest = await this.guestRepository.GetByIdAsync(query.Id, cancellationToken);
        
        return guest?.ToDto();
    }
}
