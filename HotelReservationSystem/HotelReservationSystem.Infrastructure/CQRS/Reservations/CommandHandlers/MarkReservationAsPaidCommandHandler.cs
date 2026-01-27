using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for marking a reservation as paid
/// </summary>
public class MarkReservationAsPaidCommandHandler : ICommandHandler<MarkReservationAsPaidCommand>
{
    private readonly IReservationRepository reservationRepository;

    public MarkReservationAsPaidCommandHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the command to mark a reservation as paid
    /// </summary>
    public async Task HandleAsync(MarkReservationAsPaidCommand command, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await reservationRepository.GetByIdAsync(command.ReservationId);
        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        // reate/update payment record
        // For now, just update status
        reservation.UpdateStatus(Core.Domain.Enums.ReservationStatus.Confirmed);
        await reservationRepository.UpdateAsync(reservation);
    }
}