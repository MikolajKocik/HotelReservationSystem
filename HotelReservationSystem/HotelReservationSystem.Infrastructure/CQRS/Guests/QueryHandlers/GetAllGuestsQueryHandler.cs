using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Application.Dtos.Guest;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;

/// <summary>
/// Handler for retrieving all guests
/// </summary>
public sealed class GetAllGuestsQueryHandler : IQueryHandler<GetAllGuestsQuery, IQueryable<GuestDto>>
{
    private readonly IGuestRepository guestRepository;

    public GetAllGuestsQueryHandler(IGuestRepository guestRepository)
    {
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the query to get all guests
    /// </summary>
    public async Task<IQueryable<GuestDto>> HandleAsync(GetAllGuestsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Guest> guests = await this.guestRepository.GetAllAsync();
        
        return guests.Select(g => new GuestDto
        {
            Id = g.Id,
            FirstName = g.FirstName,
            LastName = g.LastName,
            Email = g.Email,
            PhoneNumber = g.PhoneNumber,
            CreatedAt = g.CreatedAt,
            ReservationsCount = g.Reservations.Count()
        });
    }
}
