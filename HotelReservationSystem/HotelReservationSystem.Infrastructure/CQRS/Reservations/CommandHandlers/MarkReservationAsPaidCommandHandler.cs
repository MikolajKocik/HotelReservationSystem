using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for marking a reservation as paid
/// </summary>
public sealed class MarkReservationAsPaidCommandHandler : ICommandHandler<MarkReservationAsPaidCommand>
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
        Reservation? reservation = await this.reservationRepository.GetByIdAsync(command.ReservationId, cancellationToken);
        if (reservation == null)
        {
            throw new Exception("Reservation not found");
        }

        if (reservation.Status == ReservationStatus.Confirmed && reservation.Payment != null)
        {
            return;
        }

        Payment? payment = await this.paymentRepository.GetByStripePaymentIntentIdAsync(command.PaymentIntentId, cancellationToken);
        if (payment == null)
        {
            payment = new Payment(
                method: "card",
                amount: reservation.TotalPrice,
                stripePaymentIntentId: command.PaymentIntentId,
                reservationId: reservation.Id);
            
            payment.MarkAsPaid(); 
            await this.paymentRepository.CreateAsync(payment, cancellationToken);
        }
        else if (payment.IsPending)
        {
            payment.MarkAsPaid();
            await this.paymentRepository.UpdateAsync(payment, cancellationToken);
        }

        reservation.SetPayment(payment);
        reservation.UpdateStatus(ReservationStatus.Confirmed); 
        await this.reservationRepository.UpdateAsync(reservation, cancellationToken);
    }
}