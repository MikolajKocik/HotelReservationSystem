using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for canceling a reservation
/// </summary>
public sealed class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand>
{
    private readonly IReservationRepository reservationRepository;

    public CancelReservationCommandHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the command to cancel a reservation
    /// </summary>
    public async Task HandleAsync(CancelReservationCommand command, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await this.reservationRepository.GetByIdAsync(command.Id, cancellationToken);
        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        reservation.UpdateStatus(ReservationStatus.Cancelled, command.Reason);
        await this.reservationRepository.UpdateAsync(reservation, cancellationToken);
    }
}