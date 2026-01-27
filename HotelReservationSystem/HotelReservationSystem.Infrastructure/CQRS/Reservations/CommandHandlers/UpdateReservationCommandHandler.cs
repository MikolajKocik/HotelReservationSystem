using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for updating a reservation
/// </summary>
public class UpdateReservationCommandHandler : ICommandHandler<UpdateReservationCommand>
{
    private readonly IReservationRepository reservationRepository;

    public UpdateReservationCommandHandler(IReservationRepository reservationRepository)
    {
        this.reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Handles the command to update a reservation
    /// </summary>
    public async Task HandleAsync(UpdateReservationCommand command, CancellationToken cancellationToken = default)
    {
        Reservation? reservation = await reservationRepository.GetByIdAsync(command.Id);
        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        if (command.Status != reservation.Status)
        {
            reservation.UpdateStatus(command.Status, command.Reason ?? string.Empty);
        }

        // Note: Updating dates would require additional validation
        // For now, only status update is supported

        await reservationRepository.UpdateAsync(reservation);
    }
}