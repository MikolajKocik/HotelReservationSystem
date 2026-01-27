using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for confirming a reservation
/// </summary>
public class ConfirmReservationCommandHandler : ICommandHandler<ConfirmReservationCommand>
{
    private readonly IReservationRepository reservationRepository;

    public ConfirmReservationCommandHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the command to confirm a reservation
    /// </summary>
    public async Task HandleAsync(ConfirmReservationCommand command, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await reservationRepository.GetByIdAsync(command.Id);
        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        reservation.UpdateStatus(Core.Domain.Enums.ReservationStatus.Confirmed);
        await reservationRepository.UpdateAsync(reservation);
    }
}