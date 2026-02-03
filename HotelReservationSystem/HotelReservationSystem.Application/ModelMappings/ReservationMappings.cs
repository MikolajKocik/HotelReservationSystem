using HotelReservationSystem.Application.Dtos.Reservation;
using ReservationEntity = HotelReservationSystem.Core.Domain.Entities.Reservation;

namespace HotelReservationSystem.Application.ModelMappings;

public static class ReservationMappings
{
    public static ReservationDto ToDto(this ReservationEntity entity)
    {
        return new ReservationDto
        {
            Id = entity.Id,
            ArrivalDate = entity.ArrivalDate,
            DepartureDate = entity.DepartureDate,
            NumberOfGuests = entity.NumberOfGuests,
            TotalPrice = entity.TotalPrice,
            AdditionalRequests = entity.AdditionalRequests,
            Status = entity.Status,
            Reason = entity.Reason,
            CreatedAt = entity.CreatedAt,
            
            RoomId = entity.RoomId,
            RoomNumber = entity.Room?.Number ?? string.Empty,
            RoomType = entity.Room?.Type ?? default,
            RoomPricePerNight = entity.Room?.PricePerNight ?? 0,
            
            GuestId = entity.GuestId,
            GuestFirstName = entity.Guest?.FirstName ?? string.Empty,
            GuestLastName = entity.Guest?.LastName ?? string.Empty,
            GuestEmail = entity.Guest?.Email ?? string.Empty,
            GuestPhoneNumber = entity.Guest?.PhoneNumber ?? string.Empty,
            
            PaymentId = entity.PaymentId,
            PaymentStatus = entity.Payment?.Status
        };
    }
}
