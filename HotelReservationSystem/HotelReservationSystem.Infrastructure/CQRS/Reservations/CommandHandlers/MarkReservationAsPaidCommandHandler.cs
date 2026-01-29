using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for marking a reservation as paid
/// </summary>
public class MarkReservationAsPaidCommandHandler : ICommandHandler<MarkReservationAsPaidCommand>
{
    private readonly IReservationRepository reservationRepository;
    private readonly IPaymentRepository paymentRepository;

    public MarkReservationAsPaidCommandHandler(
        IReservationRepository reservationRepository,
        IPaymentRepository paymentRepository)
    {
        this.reservationRepository = reservationRepository;
        this.paymentRepository = paymentRepository;
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

        Payment? payment = await paymentRepository.GetByStripePaymentIntentIdAsync(command.PaymentIntentId);
        if (payment == null)
        {
            payment = new Payment(
                method: "card",
                amount: reservation.TotalPrice,
                stripePaymentIntentId: command.PaymentIntentId,
                reservationId: reservation.Id);

            await paymentRepository.CreateAsync(payment);
        }
        else
        {
            if (payment.IsPending)
            {
                payment.MarkAsPaid();
                await paymentRepository.UpdateAsync(payment);
            }
        }

        reservation.Payment = payment;
        reservation.UpdateStatus(ReservationStatus.Confirmed);
        await reservationRepository.UpdateAsync(reservation);
    }
}