using System.Threading;
using System.Threading.Tasks;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.Reservation.UseCases;

namespace HotelReservationSystem.Application.Interfaces;

public interface IReservationQueue
{
    Task<string> EnqueueAsync(CreateReservationCommand command);
    Task<ReservationQueueItem?> DequeueAsync(CancellationToken cancellationToken);
}