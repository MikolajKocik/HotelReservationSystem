using HotelReservationSystem.Application.Dtos.Reservation;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Web.ViewModels;

namespace HotelReservationSystem.Web.Utils.ModelMappings;

public static class ReservationMappings
{
    public static ReservationViewModel ToViewModel(this ReservationDto dto)
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

    public static ReservationListViewModel ToListViewModel(this ReservationDto dto)
    {
        return new ReservationListViewModel
        {
            Id = dto.Id,
            ArrivalDate = dto.ArrivalDate,
            DepartureDate = dto.DepartureDate,
            RoomNumber = dto.RoomNumber,
            GuestFullName = $"{dto.GuestFirstName} {dto.GuestLastName}",
            TotalPrice = dto.TotalPrice,
            Status = dto.Status.ToString()
        };
    }

    public static CreateReservationCommand ToCreateCommand(this ReservationViewModel viewModel)
    {
        return new CreateReservationCommand(
            viewModel.ArrivalDate,
            viewModel.DepartureDate,
            viewModel.RoomId,
            viewModel.GuestFirstName,
            viewModel.GuestLastName,
            viewModel.GuestEmail,
            viewModel.GuestPhoneNumber,
            viewModel.DiscountCode,
            viewModel.AdditionalRequests,
            viewModel.AcceptPrivacy,
            viewModel.NumberOfGuests
        );
    }
}
