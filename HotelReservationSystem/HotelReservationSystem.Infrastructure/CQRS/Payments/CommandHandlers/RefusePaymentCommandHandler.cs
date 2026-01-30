using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Payments.Commands;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;

namespace HotelReservationSystem.Infrastructure.CQRS.Payments.CommandHandlers;

public sealed class RefusePaymentCommandHandler : ICommandHandler<RefusePaymentCommand>
{
    private readonly IPaymentRepository paymentRepository;

    public RefusePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        this.paymentRepository = paymentRepository;
    }

    public async Task HandleAsync(RefusePaymentCommand command, CancellationToken cancellationToken = default)
    {
        Payment? payment = await this.paymentRepository.GetByStripePaymentIntentIdAsync(command.PaymentIntentId);
        if (payment == null)
            return;

        payment.MarkAsFailed();
        await this.paymentRepository.UpdateAsync(payment);
    }
}
