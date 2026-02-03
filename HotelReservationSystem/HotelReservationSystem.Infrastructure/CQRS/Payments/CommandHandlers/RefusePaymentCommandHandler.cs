using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Payments.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Infrastructure.CQRS.Payments.CommandHandlers;

public sealed class RefusePaymentCommandHandler : ICommandHandler<RefusePaymentCommand>
{
    private readonly IPaymentRepository paymentRepository;
    private readonly IReservationRepository reservationRepository;

    public RefusePaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IReservationRepository reservationRepository)
    {
        this.paymentRepository = paymentRepository;
        this.reservationRepository = reservationRepository;
    }

    public async Task HandleAsync(RefusePaymentCommand command, CancellationToken cancellationToken = default)
    {
        Payment? payment = await this.paymentRepository.GetByStripePaymentIntentIdAsync(command.PaymentIntentId, cancellationToken);
        if (payment == null)
            return;

        payment.MarkAsFailed();
        await this.paymentRepository.UpdateAsync(payment, cancellationToken);

        Reservation? reservation = await this.reservationRepository.GetByIdAsync(payment.ReservationId, cancellationToken);
        if (reservation != null && reservation.Status == ReservationStatus.Pending)
        {
            reservation.UpdateStatus(ReservationStatus.Cancelled, "Płatność nieudana");
            await this.reservationRepository.UpdateAsync(reservation, cancellationToken);
        }
    }
}
