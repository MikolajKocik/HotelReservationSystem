using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Application.ModelMappings;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;

/// <summary>
/// Handler for retrieving all guests
/// </summary>
public sealed class GetAllGuestsQueryHandler : IQueryHandler<GetAllGuestsQuery, IEnumerable<GuestDto>>
{
    private readonly IGuestRepository guestRepository;

    public GetAllGuestsQueryHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the query to get all guests
    /// </summary>
    public async Task<IEnumerable<GuestDto>> HandleAsync(GetAllGuestsQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<Guest> guests = await this.guestRepository.GetAllAsync(cancellationToken);
        
        return guests.Select(g => g.ToDto());
    }
}
