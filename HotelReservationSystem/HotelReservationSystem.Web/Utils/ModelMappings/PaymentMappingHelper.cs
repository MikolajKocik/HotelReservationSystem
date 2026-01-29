using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.Utils.ModelMappings;

public static class PaymentMappingHelper
{
    public static PaymentViewModel MapToPaymentViewModel(PaymentInfoDto dto)
    {
        return new PaymentViewModel
        {
            ReservationId = dto.ReservationId,
            TotalAmount = dto.TotalAmount,
            ClientSecret = dto.ClientSecret,
            PublishableKey = dto.PublishableKey,
            Currency = dto.Currency
        };
    }
}