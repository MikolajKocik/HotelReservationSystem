using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Payments.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;

namespace HotelReservationSystem.Infrastructure.CQRS.Payments.CommandHandlers;

/// <summary>
/// Handler for confirming payments
/// </summary>
public sealed class ConfirmPaymentCommandHandler : ICommandHandler<ConfirmPaymentCommand>
{
    private readonly IPaymentRepository paymentRepository;

    public ConfirmPaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        this.paymentRepository = paymentRepository;
    }

    /// <summary>
    /// Handles the command to confirm a payment by marking it as paid
    /// </summary>
    public async Task HandleAsync(ConfirmPaymentCommand command, CancellationToken cancellationToken = default)
    {
        Payment? payment = await this.paymentRepository.GetByStripePaymentIntentIdAsync(command.PaymentIntentId, cancellationToken);
        if (payment == null)
        {
            throw new Exception("Payment not found");
        }

        payment.MarkAsPaid();
        await this.paymentRepository.UpdateAsync(payment, cancellationToken);
    }
}