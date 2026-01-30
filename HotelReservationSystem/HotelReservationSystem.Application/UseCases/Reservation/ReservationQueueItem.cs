using HotelReservationSystem.Application.CQRS.Reservations.Commands;

namespace HotelReservationSystem.Application.Reservation.UseCases;

public record ReservationQueueItem
{
    public CreateReservationCommand Command { get; init; }
    public TaskCompletionSource<string> Tcs { get; init; } 
}