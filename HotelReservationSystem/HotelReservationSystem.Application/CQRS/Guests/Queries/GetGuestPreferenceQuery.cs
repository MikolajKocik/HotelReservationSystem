using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Core.Domain.Entities.GuestPref;

namespace HotelReservationSystem.Application.CQRS.Guests.Queries;

public record GetGuestPreferenceQuery(string email) : IQuery<List<GuestPreference>>;
