using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.Utils.ModelMappings
{
    public static class ReservationMappingHelper
    {
        public static ReservationViewModel MapToReservationViewModel(ReservationDto dto)
        {
            return new ReservationViewModel
            {
                ArrivalDate = dto.ArrivalDate,
                DepartureDate = dto.DepartureDate,
                RoomId = dto.RoomId,
                GuestFirstName = dto.GuestFirstName,
                GuestLastName = dto.GuestLastName,
                GuestEmail = dto.GuestEmail,
                GuestPhoneNumber = dto.GuestPhoneNumber, 
                DiscountCode = dto.DiscountCode,
                AdditionalRequests = dto.AdditionalRequests
            };
        }
    }
}