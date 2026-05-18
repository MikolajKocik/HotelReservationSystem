using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.CQRS.Guests.Queries;
using HotelReservationSystem.Core.Domain.Entities.GuestPref;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Guests.QueryHandlers;

public sealed class GetGuestPreferenceQueryHandler : IQueryHandler<GetGuestPreferenceQuery, List<GuestPreference>>
{
    private readonly IGuestRepository _repository;

    public GetGuestPreferenceQueryHandler(IGuestRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GuestPreference>> HandleAsync(GetGuestPreferenceQuery query, CancellationToken cancellationToken)
    {
        List<GuestPreference> preferences = await _repository.GetGuestPreferencesAsync(query.email, cancellationToken);
        
        return preferences.Any() 
            ? preferences
            : [];
    }
}